using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreShared.Service;
using CoreShared.BO;
using CoreImpl.Service;
using Moq;
using System.Linq;

namespace Tests.Service
{
    [TestClass]
    public class ReadHistoryTest
    {
        [TestMethod]
        public void TestReadHistory()
        {

            var pathService = new Mock<IPathService>(MockBehavior.Strict);
            pathService.SetupGet(x => x.StatusPath).Returns("Data/status");
            var configReader = new ConfigReader(pathService.Object);

            var history = configReader.ReadStatusList<JobHistory>("history");

            var guid = new Guid("7c0be0f1-43aa-4301-aa78-e6594c7eaf18");

            var lastRun = history.Where(x => x.Type == TraceType.Start && x.JobId == guid).OrderByDescending(x => x.Time).FirstOrDefault();

            Assert.IsNotNull(lastRun);

        }
    }
}
