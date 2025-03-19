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
    public class ApplicationStatusServiceTests
    {
        private ApplicationStatusService applicationstatusService;

        [SetUp]
        public void Setup()
        {
            var applicationstatuslist = new List<ApplicationStatusViewModel>()
            {
                new ApplicationStatusViewModel()
                {
                    ApplicationStatusId = new Guid(),
                    Description = "Test"
                }
            };
            var appplicationstatus = new ApplicationStatusViewModel()
            {
                ApplicationStatusId = new Guid(),
                Description = "Test"
            };

            IEnumerable<ApplicationStatusViewModel> applicationstatusMock = applicationstatuslist;
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            mockIApplicationStatusRepository.Setup(x => x.GetAllApplicationStatus()).Returns(Task.FromResult(applicationstatusMock));
            mockIApplicationStatusRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(appplicationstatus));
            mockIApplicationStatusRepository.Setup(x => x.Create(It.IsAny<ApplicationStatusCreateModel>())).Returns(Task.FromResult(true));
            mockIApplicationStatusRepository.Setup(x => x.Update(It.IsAny<ApplicationStatusModel>())).Returns(Task.FromResult(true));
            mockIApplicationStatusRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            applicationstatusService = new ApplicationStatusService(mockIApplicationStatusRepository.Object);
        }

        [Test]
        public async Task GetAllApplicationStatus_Test()
        {
            var result = await applicationstatusService.GetAll();
            Assert.IsNotNull(result.ToList()[0].ApplicationStatusId);
            Assert.AreEqual("Test", result.ToList()[0].Description);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await applicationstatusService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.ApplicationStatusId);
            Assert.AreEqual("Test", result.Description);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await applicationstatusService.Create(It.IsAny<ApplicationStatusCreateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await applicationstatusService.Update(It.IsAny<ApplicationStatusModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await applicationstatusService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
