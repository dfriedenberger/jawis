using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShared.BO
{
    public enum JobState
    {
        Init = 0,
        Running,
        StopedSuccessful,
        StopedWithError,
        Killed
    }
}
