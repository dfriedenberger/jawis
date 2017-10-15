using CoreImpl.Service;
using CoreShared;
using CoreShared.BO;
using CoreShared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreImpl
{
    public static class CoreFactory
    {
        private static readonly IConfigReader _configReader = new ConfigReader(new PathService());
      

        public static IConfigReader ConfigReader { get {
                return _configReader;
        } }

      
        private static readonly IConfigurationService _configurationService = new ConfigurationService(ConfigReader);

        public static IConfigurationService ConfigurationService { get {
                return _configurationService;
            }
        }

        private static readonly IStatusService _statusService = new StatusService(ConfigReader);

        public static IStatusService StatusService
        {
            get
            {
                return _statusService;
            }
        }

        public static IScheduler CreateSchedulerInstance()
        {
            return new Scheduler(ConfigurationService, StatusService, new ScheduleCalculator());
        }

        public static IJobManager CreateJobManagerInstance(Job job)
        {
            return new JobManager() {
                Config = job,
                State = JobState.Init
            };
        }
    }
}
