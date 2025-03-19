using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class MailerServiceTests
    {
        private MailerService mailerService;

        [SetUp]
        public void Setup()
        {
            var mailerview = new MailRequestModel()
            {
               To = "Test",
               Subject = "Test",
               Body = "Test",
            };

            var mockIMailerRepository = new Mock<IMailerRepository>();
            var mockIGetHtmlBodyRepository = new Mock<IGetHtmlBodyRepository>();
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockIInterviewRepository = new Mock<IInterviewRepository>();
            var mockIRecruiterJobPostRepository = new Mock<IRecruiterJobPostRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            mockIMailerRepository.Setup(x => x.SendEmail(It.IsAny<MailRequestModel>())).Returns(Task.FromResult(true));

            mailerService = new MailerService(mockIMailerRepository.Object, mockIApplicationRepository.Object, mockIInterviewerRepository.Object, mockIInterviewRepository.Object, mockIGetHtmlBodyRepository.Object, mockIRecruiterJobPostRepository.Object, mockIRecruiterRepository.Object);
        }

        [Test]
        public async Task SendEmail_Test()
        {
            var result = await mailerService.SendEmail(It.IsAny<MailRequestModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
