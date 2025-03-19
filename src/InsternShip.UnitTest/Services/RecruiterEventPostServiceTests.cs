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
    public class RecruiterEventServiceTests
    {
        private RecruiterEventPostService recruitereventService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var recruiterevent = new RecruiterEventPostViewModel()
            {
                EventPostId = new Guid(),
                FirstName = "Test",
                LastName = "Test",
                EventName = "Test",
                Location = "Test",
                MaxCandidate = 0,
                Description = "Test",
                Poster = "Test",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                PostDate = DateTime.Now,
                DeadlineDate = DateTime.Now,
            };
            var recruiterl = new List<RecruiterEventPostViewModel>();
            recruiterl.Add(recruiterevent);
            var recruitereventview = new RecruiterEventPostListViewModel()
            {
                TotalCount = 0,
                EventPostList = recruiterl
            };

            var mockIRecruiterEventPostRepository = new Mock<IRecruiterEventPostRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            var mockIEventRepository = new Mock<IEventRepository>();
            var mockIEventParticipationRepository = new Mock<IEventParticipationRepository>();
            mockIRecruiterEventPostRepository.Setup(x => x.GetAll(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(recruitereventview));
            mockIRecruiterEventPostRepository.Setup(x => x.Create(It.IsAny<RecruiterEventPostModel>())).Returns(Task.FromResult(true));
            mockIRecruiterEventPostRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<RecruiterEventPostModel>())).Returns(Task.FromResult(true));
            mockIRecruiterEventPostRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIRecruiterEventPostRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            recruitereventService = new RecruiterEventPostService(mockIRecruiterEventPostRepository.Object, 
                                                                  mockIRecruiterRepository.Object, 
                                                                  mockIEventRepository.Object, 
                                                                  mockIEventParticipationRepository.Object);
        }
        [Test]
        public async Task GetAll_Test()
        {
            var result = await recruitereventService.GetAll(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await recruitereventService.Create(It.IsAny<Guid>(), It.IsAny<CreateEventModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await recruitereventService.Update(It.IsAny<Guid>(), It.IsAny<EventUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await recruitereventService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await recruitereventService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
