using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreShared.Service
{
    public interface IScheduleCalculator
    {
        bool Match(Schedule schedule, DateTime now, DateTime last);
        bool MatchRange(Schedule schedule, DateTime now);
    }
}
