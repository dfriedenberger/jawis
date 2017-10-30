using CoreImpl;
using CoreShared.BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication.BO;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<UIJob> _jobs = new ObservableCollection<UIJob>();
        public MainWindow()
        {
            InitializeComponent();
            lbJobs.ItemsSource = _jobs;

            foreach(var job in CoreFactory.ConfigurationService.GetJobs())
            {
                var uiJob = new UIJob()
                {
                    Config = job,
                    Icon = UICanvasIcon.Init

                };
                _jobs.Add(uiJob);
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            backgroundWorker.ProgressChanged += BackgroundWorkerCycleUpdate;
            backgroundWorker.RunWorkerAsync();

        }


        private void BackgroundWorkerCycleUpdate(object sender, ProgressChangedEventArgs e)
        {
            //Read ServiceStatus
            var serverStatus = CoreFactory.StatusService.GetServiceStatus();
            if (serverStatus != null)
            {
                tbServiceStatus.Text = serverStatus.State.ToString();
                iconHeart.Fill = serverStatus.State == ServiceState.Running ? Brushes.Green : Brushes.Gray;
            }
            //Read Job Status
            var jobstatus = CoreFactory.StatusService.GetJobStatus();

            foreach(var job in _jobs)
            {
                var status = jobstatus.FirstOrDefault(s => s.Id.Equals(job.Config.Id));
                UICanvasIcon icon = null;
                if(status == null)
                {
                    //Todo Is Disabled
                    icon = UICanvasIcon.Waiting;
                }
                else
                {
                    switch(status.State)
                    {
                        case JobState.Running:
                            icon = UICanvasIcon.Running;
                            break;
                        default:
                            icon = UICanvasIcon.Error;
                            break;
                    }
                }

                if (job.Icon != icon)
                {
                    job.Icon = icon;
                    CollectionViewSource.GetDefaultView(_jobs).Refresh();
                }
            }


        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                //Do your stuff here
                Thread.Sleep(1000);
                worker.ReportProgress(0);
            }
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void mnuOptions_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindows dialog = new OptionsWindows();


            dialog.Options = CoreFactory.ConfigurationService.GetOptions();

            if (dialog.ShowDialog() == true)
            {
                //set config
                CoreFactory.ConfigurationService.SetOptions(dialog.Options);
            }
        }

        private void mnuAbount_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow dialog = new AboutWindow();
            dialog.ShowDialog();
        }
     
        

        private void MenuItemDetails_Click(object sender, RoutedEventArgs e)
        {
            //Details
            var job = lbJobs.SelectedItem as UIJob;
            if (job == null) return;
            var history = CoreFactory.StatusService.GetHistory();

            var lst = new ObservableCollection<UIHistory>();

            foreach(var h in history.Where(x => x.JobId == job.Config.Id).OrderBy(y => y.Time))
            {
                lst.Add(new UIHistory()
                {
                    Title = String.Format("{0:dd.MM.yyyy HH:mm:ss} {1} {2}", h.Time, h.Type, h.Message)
                });
            }

            var dialog = new JobDetails(lst);
           
           
            dialog.ShowDialog();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var job = lbJobs.SelectedItem as UIJob;
            if (job == null) return;

            CoreFactory.ConfigurationService.DeleteJob(job.Config);
            _jobs.Remove(job);
                //refresh
           
        }

        private void MenuItemProperties_Click(object sender, RoutedEventArgs e)
        {
            var job = lbJobs.SelectedItem as UIJob;
            if (job == null) return;

            var dialog = new JobPropertyWindow();
            dialog.Job = job;
            if (dialog.ShowDialog() == true)
            {
                //modify Job properties
                CoreFactory.ConfigurationService.UpdateJob(dialog.Job.Config);
                //refresh
                CollectionViewSource.GetDefaultView(_jobs).Refresh();
            }
        }

        private void btnNewJob_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new JobPropertyWindow();

            dialog.Job = new UIJob()
            {
                Config = new Job()
                {
                    Id = Guid.NewGuid(),
                    Type = JobType.Java,
                    Executable = "",
                    Arguments = "",
                    WorkingDirectory = "",
                    Schedule = new Schedule()
                    {
                        Type = ScheduleType.Ever,
                        Minute = 1,
                        TimeHour = 8,
                        TimeMinute = 0
                    }
                },
                Icon = UICanvasIcon.Init
            };

            if (dialog.ShowDialog() == true)
            {
                //add job
                CoreFactory.ConfigurationService.AddJob(dialog.Job.Config);
                _jobs.Add(dialog.Job);
            }
        }
    }
}
