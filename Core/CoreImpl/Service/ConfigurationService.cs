using CoreShared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreImpl.Service
{
    class ConfigurationService : IConfigurationService
    {
        private IConfigReader _configReader;
        public ConfigurationService(IConfigReader _configReader)
        {
            this._configReader = _configReader;
        }

        public void AddJob(Job job)
        {
            var jobs = _configReader.ReadConfig<List<Job>>("jobs");
            if (jobs == null)
                jobs = new List<Job>();
            jobs.Add(job);
            _configReader.WriteConfig("jobs", jobs);
        }

     

        public IList<Job> GetJobs()
        {
            var jobs = _configReader.ReadConfig<List<Job>>("jobs");
            if (jobs == null)
                jobs = new List<Job>();
            return jobs;
        }

        public void UpdateJob(Job job)
        {
            var jobs = _configReader.ReadConfig<List<Job>>("jobs");
            var ix = jobs.Select((j , index) => new { j, index }).Single(x => x.j.Id.Equals(job.Id)).index;

            jobs[ix] = job;

            _configReader.WriteConfig("jobs", jobs);

        }
        public void DeleteJob(Job job)
        {
            var jobs = _configReader.ReadConfig<List<Job>>("jobs");
            var ix = jobs.Select((j, index) => new { j, index }).Single(x => x.j.Id.Equals(job.Id)).index;
            jobs.RemoveAt(ix);
            _configReader.WriteConfig("jobs", jobs);

        }
    }
}
