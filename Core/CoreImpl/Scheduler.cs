using CoreShared;
using CoreShared.BO;
using CoreShared.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreImpl
{
    class Scheduler : IScheduler
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfigurationService _configurationService;
        private readonly IScheduleCalculator _scheduleCalculator;
        private readonly IStatusService _statusService;

        private IList<IJobManager> RunningJobs = new List<IJobManager>();
        public Scheduler(IConfigurationService configurationService, IStatusService statusService,IScheduleCalculator scheduleCalculator)
        {
            ShouldRun = true;
            _configurationService = configurationService;
            _statusService = statusService;
            _scheduleCalculator = scheduleCalculator;
        }
        public bool ShouldRun { get; set; }

        private int _lastMinute = -1;

        public void Run()
        {
            _statusService.Flush(ServiceState.Start);
            try
            {

                while (ShouldRun)
                {
                    var current = DateTime.Now;
                    if (_lastMinute != current.Minute) //Nur einmal pro Minute ausführen
                    {
                        _lastMinute = current.Minute;
                        _statusService.Flush(ServiceState.Running);
                        Step(current);
                        _statusService.Flush(ServiceState.Waiting);

                    }
                    //Wait before next step

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                Kill();
                _statusService.Flush(ServiceState.Stop);

            }
            catch (Exception e)
            {
                //Catch all Exception
                _log.Error(e);
                _statusService.Flush(ServiceState.Error);
                return;
            }
        }

        private void Kill()
        {
            //CommitState
            var statusList = new List<JobStatus>();
            for (int i = RunningJobs.Count - 1; i >= 0; i--)
            {
               
            
                if (RunningJobs[i].State == JobState.Running)
                {
                    //Kill
                    _log.InfoFormat("Kill job {0}", RunningJobs[i].Config.Executable);
                    RunningJobs[i].Kill();
                }

                var jobStatus = new JobStatus()
                {
                    Id = RunningJobs[i].Config.Id,
                    State = RunningJobs[i].State
                };
                statusList.Add(jobStatus);

                _statusService.TraceStop(RunningJobs[i]);

                RunningJobs.RemoveAt(i);
            }
            _statusService.Flush(statusList);
        }


        private void Step(DateTime now)
        {
            _log.DebugFormat("Cycle {0:D2}:{1:D2} Uhr / Running Jobs {2}", now.Hour, now.Minute, RunningJobs.Count);

            var options = _configurationService.GetOptions();
            var jobs = _configurationService.GetJobs();
            var history = _statusService.GetHistory();


            //Look at Jobs and dequeue
            for (int i = RunningJobs.Count - 1; i >= 0; i--)
            {

                switch(RunningJobs[i].State)
                {
                    case JobState.Running:
                        var config = jobs.SingleOrDefault(j => j.Id == RunningJobs[i].Config.Id);
                        if ((config == null) || (config.Enabled == false) || 
                            !_scheduleCalculator.MatchRange(config.Schedule,now))
                        {
                            //Must Kill 
                            _log.InfoFormat("Kill job {0}", RunningJobs[i].Config.Executable);
                            RunningJobs[i].Kill();
                            _statusService.TraceStop(RunningJobs[i]);
                            RunningJobs.RemoveAt(i);
                        }

                        break;
                    case JobState.StopedSuccessful:
                        _log.InfoFormat("Job stoped successful {0} ", RunningJobs[i].Config.Executable);
                        _statusService.TraceStop(RunningJobs[i]);
                        RunningJobs.RemoveAt(i);
                        break;
                    case JobState.StopedWithError:
                        _log.ErrorFormat("Job stoped with errors {0}", RunningJobs[i].Config.Executable);
                        _statusService.TraceStop(RunningJobs[i]);
                        RunningJobs.RemoveAt(i);
                        break;
                }

            }

          

            foreach (var job in jobs)
            {
                

                var last = DateTime.MinValue;

                var lastRun = history.Where(x => x.Type == TraceType.Start && x.JobId == job.Id).OrderByDescending(x => x.Time).FirstOrDefault();
                if (lastRun != null)
                    last = lastRun.Time;

                //Should the job run?
                _log.DebugFormat("Check {0}", job.Executable);

                if (job.Enabled == false) continue;

                if (!_scheduleCalculator.Match(job.Schedule, now, last)) continue;

                //Todo check if jobs run
                var current = RunningJobs.SingleOrDefault(j => j.Config.Id.Equals(job.Id));
                if(current != null)
                {
                    _log.DebugFormat("Job {0} is still running, do not start another one", current.Config.Executable);
                    continue;
                }

                _log.InfoFormat("Try Start Job {0}", job.Executable);
                var jobManager = CoreFactory.CreateJobManagerInstance(job, options);
                jobManager.Start();
                RunningJobs.Add(jobManager);
                _statusService.TraceStart(jobManager);
               _log.InfoFormat("Job {0} Started", job.Executable);
            }

            //CommitState
            var statusList = new List<JobStatus>();
            foreach (var runningJob in RunningJobs)
            {
                statusList.Add(new JobStatus() {
                    Id = runningJob.Config.Id,
                    State  = runningJob.State
                });
            }
            _statusService.Flush(statusList);
        }
    }
}
