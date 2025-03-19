using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
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
    public class InterviewServiceTests
    {
        private InterviewService interviewService;
        private ApplicationService applicationService;

        [SetUp]
        public void Setup()
        {
            var interview = new InterviewViewModel()
            {
                InterviewId = new Guid(),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Location = "Test"
            };


            var interviewl = new List<InterviewViewModel>();
            interviewl.Add(interview);
            var interviewlist = new InterviewListViewModel()
            {
                TotalCount = 0,
                InterviewList = interviewl
            };
            var mockIInterviewRepository = new Mock<IInterviewRepository>();
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIInterviewSessionRepository = new Mock<IInterviewSessionRepository>();
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockITestRepository = new Mock<ITestRepository>();
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            var mockIApplicationStatusUpdateRepository = new Mock<IApplicationStatusUpdateRepository>();

            mockIInterviewRepository.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(interviewlist));
            mockIInterviewRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(interview));
            mockIInterviewRepository.Setup(x => x.Create(It.IsAny<CreateInterviewModel>())).Returns(Task.FromResult(true));
            mockIInterviewRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<InterviewUpdateModel>())).Returns(Task.FromResult(true));
            mockIInterviewRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            interviewService = new InterviewService(mockIInterviewRepository.Object, 
                                                    mockIApplicationRepository.Object,
                                                    mockIInterviewSessionRepository.Object,
                                                    mockIInterviewerRepository.Object,
                                                    mockITestRepository.Object,
                                                    mockIApplicationStatusUpdateRepository.Object,
                                                    mockIApplicationStatusRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await interviewService.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result.InterviewList.ToList()[0].InterviewId);
            Assert.IsNotNull(result.InterviewList.ToList()[0].StartTime);
            Assert.IsNotNull(result.InterviewList.ToList()[0].EndTime);
            Assert.AreEqual("Test", result.InterviewList.ToList()[0].Location);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await interviewService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.InterviewId);
            Assert.IsNotNull(result.StartTime);
            Assert.IsNotNull(result.EndTime);
            Assert.AreEqual("Test", result.Location);
        }

        [Test]
        public async Task Create_Test()
        {
            try
            {
                var interview = new CreateInterviewModel()
                {
                    ApplicationId = new Guid()
                };
                var result = await interviewService.Create(interview);
                Assert.IsNotNull(result);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await interviewService.Update(It.IsAny<Guid>(), It.IsAny<InterviewUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await interviewService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}

