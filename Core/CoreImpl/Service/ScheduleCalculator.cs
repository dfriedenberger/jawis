using CoreShared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreShared.BO;

namespace CoreImpl.Service
{
    class ScheduleCalculator : IScheduleCalculator
    {
        public bool Match(Schedule schedule, DateTime now, DateTime last)
        {
            TimeSpan span = now.Subtract(last);
            switch (schedule.Type)
            {
                case ScheduleType.Ever:
                    return true;
                case ScheduleType.EveryXMinute:
                    return span.TotalMinutes > schedule.Minute;
                case ScheduleType.DailyAtX:
                    return (schedule.TimeHour == now.Hour && schedule.Minute == now.Minute);
            }
            return false;
        }
    }
}
