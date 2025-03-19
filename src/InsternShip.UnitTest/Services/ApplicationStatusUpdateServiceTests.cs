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
    public class ApplicationStatusUpdateServiceTests
    {
        private ApplicationStatusUpdateService applicationstatusupdateService;

        [SetUp]
        public void Setup()
        {
            var applicationstatusupdate = new List<ApplicationStatusUpdateViewModel>()
            {
                new ApplicationStatusUpdateViewModel()
                {
                    StatusId = new Guid(),
                    ApplicationId = new Guid(),
                    LatestUpdate = DateTime.Now
                }
            };
            var appplicationstatusupdateview = new ApplicationStatusUpdateViewModel()
            {
                StatusId = new Guid(),
                ApplicationId = new Guid(),
                LatestUpdate = DateTime.Now,
            };

            var appplicationstatusupdate = new ApplicationStatusUpdateModel()
            {
                ApplicationStatusUpdateId = new Guid(),
                StatusId = new Guid(),
                ApplicationId = new Guid(),
                LatestUpdate = DateTime.Now,
            };

            IEnumerable<ApplicationStatusUpdateViewModel> applicationstatusupdateMock = applicationstatusupdate;
            var mockIApplicationStatusUpdateRepository = new Mock<IApplicationStatusUpdateRepository>();
            var mockIApplicationRepository = new Mock<IApplicationRepository>();
            var mockIApplicationStatusRepository = new Mock<IApplicationStatusRepository>();
            mockIApplicationStatusUpdateRepository.Setup(x => x.GetAllApplicationStatusUpdate()).Returns(Task.FromResult(applicationstatusupdateMock));
            mockIApplicationStatusUpdateRepository.Setup(x => x.GetAllByApplicationId(It.IsAny<Guid>())).Returns(Task.FromResult(applicationstatusupdateMock));
            mockIApplicationStatusUpdateRepository.Setup(x => x.GetByApplicationIdAndStatus(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(appplicationstatusupdateview));
            mockIApplicationStatusUpdateRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(appplicationstatusupdateview));
            mockIApplicationStatusUpdateRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).Returns(Task.FromResult(appplicationstatusupdate));
            mockIApplicationStatusUpdateRepository.Setup(x => x.Create(It.IsAny<ApplicationStatusUpdateCreateModel>())).Returns(Task.FromResult(true));
            mockIApplicationStatusUpdateRepository.Setup(x => x.Update(It.IsAny<ApplicationStatusUpdateModel>())).Returns(Task.FromResult(true));
            mockIApplicationStatusUpdateRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            applicationstatusupdateService = new ApplicationStatusUpdateService(mockIApplicationStatusUpdateRepository.Object, mockIApplicationRepository.Object, mockIApplicationStatusRepository.Object);
        }

        [Test]
        public async Task GetAllApplicationStatusUpdate_Test()
        {
            var result = await applicationstatusupdateService.GetAll();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToList()[0].StatusId);
            Assert.IsNotNull(result.ToList()[0].ApplicationId);
            Assert.IsNotNull(result.ToList()[0].LatestUpdate);
        }

        [Test]
        public async Task GetAllByApplycationId_Test()
        {
            var result = await applicationstatusupdateService.GetAllByApplicationId(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToList()[0].StatusId);
            Assert.IsNotNull(result.ToList()[0].ApplicationId);
            Assert.IsNotNull(result.ToList()[0].LatestUpdate);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await applicationstatusupdateService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.StatusId);
            Assert.IsNotNull(result.ApplicationId);
            Assert.IsNotNull(result.LatestUpdate);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await applicationstatusupdateService.Create(It.IsAny<ApplicationStatusUpdateCreateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await applicationstatusupdateService.Update(It.IsAny<ApplicationStatusUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await applicationstatusupdateService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
