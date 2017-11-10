using CoreShared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreImpl.Service
{
    public class ScheduleCalculator : IScheduleCalculator
    {
        public bool Match(Schedule schedule, DateTime now, DateTime last)
        {
            TimeSpan span = now.Subtract(last);

            //check Cycle
            if (!schedule.Ever)
            {
                if (span.TotalMinutes < schedule.CycleValue * (int)schedule.CycleUnit)
                    return false;
            }

            return MatchRange(schedule, now);
        }

        public bool MatchRange(Schedule schedule, DateTime now)
        {
            if (!schedule.Continuous)
            {
                if (now.Hour < schedule.TimeHourFrom) return false;
                if ((now.Hour == schedule.TimeHourFrom) && (now.Minute < schedule.TimeMinuteFrom)) return false;
                if (schedule.TimeHourTo < now.Hour) return false;
                if ((schedule.TimeHourTo == now.Hour) && (schedule.TimeMinuteTo < now.Minute)) return false;
            }

            return true;
        }
    }
}
