using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared.BO
{
    public class Job
    {
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public Schedule Schedule { get; set; }
        public JobType Type { get; set; }
        public Guid Id { get; set; }
    }
}
