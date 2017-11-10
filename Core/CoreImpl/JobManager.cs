using CoreShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;
using System.Threading;
using System.Diagnostics;
using log4net;
using System.IO;
using System.Reflection;

namespace CoreImpl
{
    class JobManager : IJobManager
    {
        private Thread _thread = null;
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool _shouldRun;
        private readonly Guid _id = Guid.NewGuid();

        public Job Config { get; set; }
      
        public JobState State { get; set; }

        public Guid Id { get { return _id; } }

        public DateTime StopTime { get; private set; }
        public Options Options { get; internal set; }

        public void Kill()
        {
            if (_thread == null) return;
            _shouldRun = false;

            if (!_thread.Join(TimeSpan.FromSeconds(5)))
            {
                _log.ErrorFormat("Could not Stop Thread for {0}",Config.Executable);
                _thread.Abort();
                StopTime = DateTime.Now;
            }
            State = JobState.Killed;
            _thread = null;
        }

        private void SendCtrlC(int pid)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            var codebasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if(!Directory.Exists(codebasePath))
            {
                _log.ErrorFormat("Could not find Directory {0}", codebasePath);
                return;
            }
            var StopProcessGracefully = Path.Combine(codebasePath, "Jawis.StopProcessGracefully.exe");

            if (!File.Exists(StopProcessGracefully))
            {
                _log.ErrorFormat("Could not find File {0}", StopProcessGracefully);
                return;
            }

            startInfo.FileName = StopProcessGracefully;
            startInfo.Arguments = " -pid " + pid;
            startInfo.WorkingDirectory = "";

            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            using (Process exeProcess = Process.Start(startInfo))
            {
                // signature to ErrorDataReceived and OutputDataReceived
                exeProcess.ErrorDataReceived += (sender, errorLine) => { if (errorLine.Data != null) _log.Error(errorLine.Data); };
                exeProcess.OutputDataReceived += (sender, outputLine) => { if (outputLine.Data != null) _log.Info(outputLine.Data); };

                exeProcess.BeginErrorReadLine();
                exeProcess.BeginOutputReadLine();

                if (!exeProcess.WaitForExit(1000))
                {
                    exeProcess.Kill();
                }
            }
        }

        public void Start()
        {
            _shouldRun = true;
            _thread = new Thread(new ThreadStart(Run));

            // Start the thread
            _thread.Start();
        }

        private void Run()
        {
            State = JobState.Running;
            ProcessStartInfo startInfo = new ProcessStartInfo();


            if (Options.UseFromPath)
                startInfo.FileName = "java";
            else
                startInfo.FileName = Options.JavaBinary;

            startInfo.Arguments = " " + "-jar "+Config.Executable+" "+Config.Arguments;
            startInfo.WorkingDirectory = Config.WorkingDirectory;

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    // You can pass any delegate that matches the appropriate 
                    // signature to ErrorDataReceived and OutputDataReceived
                    exeProcess.ErrorDataReceived += (sender, errorLine) => { if (errorLine.Data != null) _log.Error(errorLine.Data); };
                    exeProcess.OutputDataReceived += (sender, outputLine) => { if (outputLine.Data != null) _log.Info(outputLine.Data); };

                    exeProcess.BeginErrorReadLine();
                    exeProcess.BeginOutputReadLine();

                    while (_shouldRun)
                    {
                        if (exeProcess.WaitForExit(1000)) break;
                    }

                    if (!_shouldRun)
                    {
                        SendCtrlC(exeProcess.Id);
                        if (!exeProcess.WaitForExit(1000))
                        {
                            exeProcess.Kill();
                        }
                    }


                    if (exeProcess.ExitCode != 0)
                    {
                        _log.ErrorFormat("Process exited with {0}", exeProcess.ExitCode);
                        State = JobState.StopedWithError;
                    }
                    else //Alles OK
                    {
                        State = JobState.StopedSuccessful;
                    }
                }
            }
            catch (Exception e)
            {
                State = JobState.StopedWithError;
                _log.Error(e);
            }
            StopTime = DateTime.Now;
        }

      
    }
}
