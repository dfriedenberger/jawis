using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared.BO
{
    public class JobStatus
    {
        public Guid Id { get; set; }
        public JobState State { get; set; }
    }
}
