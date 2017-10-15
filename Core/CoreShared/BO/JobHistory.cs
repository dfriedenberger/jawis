using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared.BO
{
    public class JobHistory
    {
        public TraceType Type { get; set; }
        public Guid ManagerId { get; set; }
        public Guid JobId { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }
}
