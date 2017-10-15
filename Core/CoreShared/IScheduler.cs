using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared
{
    public interface IScheduler
    {
        bool ShouldRun { get; set; }

        void Run();
    }
}
