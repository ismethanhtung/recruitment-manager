using AutoMapper;
using Microsoft.Extensions.Configuration;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class EventParticipationServiceTests
    {
        private EventParticipationService eventparticipationService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);
            var eventparticipation = new EventParticipationViewModel()
            {
                ParticipationId = new Guid(),
                EventPostId = new Guid(),
                CandidateId = new Guid(),
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                PhoneNumber = "Test",
                Description = "Test",
                Education = "Test",
                Experience = "Test",
                Language = "Test",
                Skillsets = "Test",
                Status = false,
            };
            var eventparticipationL = new List<EventParticipationViewModel>();
            eventparticipationL.Add(eventparticipation);
            var eventparticipationlist = new EventParticipationListViewModel()
            {
                TotalCount = 0,
                EventParticipationList = eventparticipationL

            };

            var eventpostparticipationlist = new EventPostParticipationListViewModel()
            {
                TotalCount = 0
            };

            var mockIEventParticipationRepository = new Mock<IEventParticipationRepository>();
            var mockIRecruiterEventPostRepository = new Mock<IRecruiterEventPostRepository>();
            mockIEventParticipationRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(eventparticipationlist));
            mockIEventParticipationRepository.Setup(x => x.GetAllEventOfCandidate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(eventpostparticipationlist));
            mockIEventParticipationRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(eventparticipation));
            mockIEventParticipationRepository.Setup(x => x.Update(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIEventParticipationRepository.Setup(x => x.UpdateByCEId(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIEventParticipationRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIEventParticipationRepository.Setup(x => x.DeleteByCEid(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
            eventparticipationService = new EventParticipationService(mockIEventParticipationRepository.Object, 
                                                                      mockIRecruiterEventPostRepository.Object
                                                                      );
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await eventparticipationService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.EventParticipationList.ToList()[0].CandidateId);
            Assert.IsNotNull(result.EventParticipationList.ToList()[0].EventPostId);
            Assert.IsNotNull(result.EventParticipationList.ToList()[0].ParticipationId);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].FirstName);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].LastName);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].Email);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].PhoneNumber);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].Description);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].Education);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].Experience);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].Language);
            Assert.AreEqual("Test", result.EventParticipationList.ToList()[0].Skillsets);
            Assert.AreEqual(false, result.EventParticipationList.ToList()[0].Status);
        }

        [Test]
        public async Task GetAllEventOfCandidate_Test()
        {
            var result = await eventparticipationService.GetAllEventOfCandidate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById()
        {
            var result = await eventparticipationService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CandidateId);
            Assert.IsNotNull(result.EventPostId);
            Assert.IsNotNull(result.ParticipationId);
            Assert.AreEqual("Test", result.FirstName);
            Assert.AreEqual("Test", result.LastName);
            Assert.AreEqual("Test", result.Email);
            Assert.AreEqual("Test", result.PhoneNumber);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual("Test", result.Education);
            Assert.AreEqual("Test", result.Experience);
            Assert.AreEqual("Test", result.Language);
            Assert.AreEqual("Test", result.Skillsets);
            Assert.AreEqual(false, result.Status);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await eventparticipationService.Update(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task UpdateByCEId_Test()
        {
            var result = await eventparticipationService.UpdateByCEId(It.IsAny<Guid>(), It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await eventparticipationService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task DeleteByCEid_Test()
        {
            var result = await eventparticipationService.DeleteByCEid(It.IsAny<Guid>(), It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
