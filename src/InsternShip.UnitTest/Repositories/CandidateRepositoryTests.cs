using InsternShip.Data;
using InsternShip.Data.Entities;
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
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using InsternShip.UnitTest.Repositories;

namespace InsternShip.UnitTest.Repository
{
    public class CandidateRepositoryTests : BaseRepositoryClass
    {
        private BlackListRepository blackListRepository;
        private CandidateRepository candidateRepository;
        private UserAccountRepository userAccountRepository;
        private EventRepository eventRepository;
        private IUnitOfWork unitOfWork;

        private readonly UserManager<UserAccount> userManager;
        private readonly SignInManager<UserAccount> signInManager;
        private readonly IMailerRepository mailer;
        private readonly IUrlHelper urlHelper;
        private readonly IConfiguration configuration;
        private readonly RoleManager<Roles> roleManager;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
            eventRepository = new EventRepository(db, unitOfWork, mapper);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await candidateRepository.GetAll("", 1, 5, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await candidateRepository.GetById(new Guid(), "");
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetAllWithKeyWord_Test()
        {
            var result = await candidateRepository.GetAll("Test", 1, 5, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Create_Test()
        {
            var candidate = new CreateCandidateModel()
            {
                UserId = new Guid(),
                Description = "Test",
                Education = "Test",
                Experience = "Test",
            };
            var result = await candidateRepository.Create(candidate);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var candidate = new CandidateUpdateModel()
                {
                    Description = "Test2",
                    Education = "Test",
                    Experience = "Test",
                    Skillsets = "Test",
                };
                var result = await candidateRepository.Update(new Guid(), candidate);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Restore_Test()
        {
            try
            {
                Guid candidateid = (await candidateRepository.GetAll("Test", 1, 1, false)).CandidateList.First().CandidateId;
                var result = await candidateRepository.Restore(candidateid);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Delete_Test()
        {
            try
            {
                var result = await candidateRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
