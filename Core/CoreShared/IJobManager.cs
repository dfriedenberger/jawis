using CoreShared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared
{
    public interface IJobManager
    {
        Job Config { get; set; }
        JobState State { get; set; }
        Guid Id { get; }

        DateTime StopTime { get; }

        void Start();
        void Kill();
    }
}
