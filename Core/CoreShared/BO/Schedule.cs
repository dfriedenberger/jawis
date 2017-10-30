namespace CoreShared.BO
{
    public class Schedule
    {
        public ScheduleType Type { get; set; }

        public int CycleValue { get; set; }
        public CycleUnit CycleUnit { get; set; }

        public int TimeMinute { get; set; }
        public int TimeHour { get; set; }

    }
}