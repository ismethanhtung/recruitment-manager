using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class ExportDataServiceTests
    {
        private ExportDataService exportdataService;

        [SetUp]
        public void Setup()
        {
            Byte[] array = new Byte[64];
            var mockIExportDataRepository = new Mock<IExportDataRepository>();
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            var mockIApplicationStatusUpdateRepository = new Mock<IApplicationStatusUpdateRepository>();
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIEventParticipationRepository = new Mock<IEventParticipationRepository>();
            var mockIInterviewSessionRepository = new Mock<IInterviewSessionRepository>();
            var mockIInterviewRepository = new Mock<IInterviewRepository>();
            mockIExportDataRepository.Setup(x => x.Export(It.IsAny<List<CandidateReportModel>>(), It.IsAny<List<RecruitmentReportModel>>(), It.IsAny<List<EventReportModel>>(), It.IsAny<List<InterviewReportModel>>())).Returns(Task.FromResult(array));
            exportdataService = new ExportDataService(mockIExportDataRepository.Object,
                                                      mockIApplicationRepository.Object,
                                                      mockIApplicationStatusRepository.Object,
                                                      mockIApplicationStatusUpdateRepository.Object,
                                                      mockICandidateRepository.Object,
                                                      mockIEventParticipationRepository.Object,
                                                      mockIInterviewSessionRepository.Object,
                                                      mockIInterviewRepository.Object);
        }

        [Test]
        public async Task CandidateReport_Test()
        {
            var result = await exportdataService.CandidateReport();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task RecruitmentReport_Test()
        {
            var result = await exportdataService.RecruitmentReport();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task EventReport_Test()
        {
            var result = await exportdataService.EventReport();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task InterviewReport_Test()
        {
            var result = await exportdataService.InterviewReport();
            Assert.IsNotNull(result);
        }
    }
}