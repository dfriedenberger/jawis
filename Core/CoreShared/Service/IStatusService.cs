using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreShared.Service
{
    public interface IStatusService
    {
        void Flush(IList<JobStatus> statusList);
        void Flush(ServiceState start);
        ServiceStatus GetServiceStatus();
        IList<JobStatus> GetJobStatus();
        void TraceStop(IJobManager jobManager);
        void TraceStart(IJobManager jobManager);
        IList<JobHistory> GetHistory();

    }
}
