using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreShared.Service;
using CoreImpl;
using CoreImpl.Service;
using CoreShared.BO;

namespace Tests.Service
{
    [TestClass]
    public class ScheduleCalculatorTest
    {
        [TestMethod]
        public void TestMatchEver()
        {
            IScheduleCalculator scheduleCalculator = new ScheduleCalculator();

            Schedule schedule = new Schedule()
            {
                Ever = true,
                Continuous = true
            };

            DateTime last = DateTime.ParseExact("2009-05-08 14:40", "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);


            DateTime now = DateTime.ParseExact("2009-05-08 14:41", "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            Assert.IsTrue(scheduleCalculator.Match(schedule, now, last));

        }

        [TestMethod]
        public void TestMatchEverWithRange()
        {
            IScheduleCalculator scheduleCalculator = new ScheduleCalculator();

            Schedule schedule = new Schedule()
            {
                Ever = true,
                Continuous = false, //von 08:00 bis 09:00
                TimeHourFrom = 8,
                TimeMinuteFrom = 0,
                TimeHourTo = 9,
                TimeMinuteTo = 0
            };

            DateTime last = DateTime.ParseExact("2009-05-08 14:40", "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);


            DateTime now = DateTime.ParseExact("2009-05-08 14:41", "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            Assert.IsFalse(scheduleCalculator.Match(schedule, now, last));

        }


        [TestMethod]
        public void TestMatchEachTwoHours()
        {
            IScheduleCalculator scheduleCalculator = new ScheduleCalculator();

            Schedule schedule = new Schedule()
            {
                Ever = false,
                CycleValue = 2,
                CycleUnit = CycleUnit.hours,
                Continuous = false, //von 08:00 bis 09:00
                TimeHourFrom = 8,
                TimeMinuteFrom = 0,
                TimeHourTo = 9,
                TimeMinuteTo = 0
            };

            DateTime last = DateTime.ParseExact("2009-05-08 06:20", "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);


            DateTime now = DateTime.ParseExact("2009-05-08 08:41", "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            Assert.IsTrue(scheduleCalculator.Match(schedule, now, last));

        }

    }
}
