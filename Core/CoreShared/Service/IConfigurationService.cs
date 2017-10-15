using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreShared.Service
{
    public interface IConfigurationService
    {
        void AddJob(Job job);
        void UpdateJob(Job job);
        IList<Job> GetJobs();
        void DeleteJob(Job job);
    }
}
