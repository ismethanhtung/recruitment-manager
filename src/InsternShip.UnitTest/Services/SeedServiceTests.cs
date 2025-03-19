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
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace InsternShip.UnitTest.Services
{
    public class SeedServiceTests
    {
        private SeedService seedService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var mockISeedRepository = new Mock<ISeedRepository>();
            var mockICandidateRepository = new Mock<ICandidateRepository>();
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockIRecruiterRepository = new Mock<IRecruiterRepository>();
            mockISeedRepository.Setup(x => x.CreateRole()).Returns(Task.FromResult(true));
            mockISeedRepository.Setup(x => x.AddUserToRole(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new Guid()));
            mockISeedRepository.Setup(x => x.CreateUser(It.IsAny<CreateUserModel>())).Returns(Task.FromResult(IdentityResult.Success));
            seedService = new SeedService(mockISeedRepository.Object,
                                                    mockICandidateRepository.Object,
                                                    mockIInterviewerRepository.Object,
                                                    mockIRecruiterRepository.Object);
        }

        [Test]
        public async Task CreateUser_Test()
        {
            var result = await seedService.CreateUser(It.IsAny<CreateUserModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(IdentityResult.Success, result);
        }

        [Test]
        public async Task CreateRoles_Test()
        {
            var result = await seedService.CreateRole();
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await seedService.AddUserToRole(It.IsAny<string>(), "");
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
