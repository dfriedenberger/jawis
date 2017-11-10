namespace CoreShared.BO
{
    public class Schedule
    {
        //Cycle
        public int CycleValue { get; set; }
        public CycleUnit CycleUnit { get; set; }
        public bool Ever { get; set; }

        //Range
        public int TimeMinuteFrom { get; set; }
        public int TimeHourFrom { get; set; }
        public int TimeMinuteTo { get; set; }
        public int TimeHourTo { get; set; }
        public bool Continuous { get; set; }
    }
}