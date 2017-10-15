using CoreShared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;
using CoreShared;

namespace CoreImpl.Service
{
    class StatusService : IStatusService
    {
        private IConfigReader _configReader;
        public StatusService(IConfigReader configReader)
        {
            _configReader = configReader;
        }

        public void Flush(ServiceState state)
        {
            var serviceStatus = new ServiceStatus()
            {
                TimeStamp = DateTime.Now,
                State = state
            };
            _configReader.WriteStatus("service", serviceStatus);
        }

        public void Flush(IList<JobStatus> statusList)
        {
            _configReader.WriteStatus("jobs" , statusList);
        }

      
        public IList<JobStatus> GetJobStatus()
        {
            var lst = _configReader.ReadStatus<List<JobStatus>>("jobs");
            if (lst == null)
                lst = new List<JobStatus>();
            return lst;
        }

        public ServiceStatus GetServiceStatus()
        {
            return _configReader.ReadStatus<ServiceStatus>("service");
        }

        public void TraceStart(IJobManager jobManager)
        {
            var history = new JobHistory()
            {
                Type = TraceType.Start,
                JobId = jobManager.Config.Id,
                ManagerId = jobManager.Id,
                Time = DateTime.Now,
                Message = ""
            };
            _configReader.AppendStatus("history", history);
        }

        public void TraceStop(IJobManager jobManager)
        {
            var history = new JobHistory()
            {
                Type = TraceType.Stop,
                JobId = jobManager.Config.Id,
                ManagerId = jobManager.Id,
                Time = jobManager.StopTime,
                Message = jobManager.State.ToString()
            };
            _configReader.AppendStatus("history", history);
           
        }
        public IList<JobHistory> GetHistory()
        {
            return _configReader.ReadStatusList<JobHistory>("history");
        }


    }
}
