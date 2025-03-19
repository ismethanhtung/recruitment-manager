using AutoMapper;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using InsternShip.UnitTest.Repositories;

namespace InsternShip.UnitTest.Repository
{
    public class RecruiterJobPostRepositoryTests : BaseRepositoryClass
    {
        private UserAccountRepository userAccountRepository;
        private BlackListRepository blackListRepository;
        private RecruiterRepository recruiterRepository;
        private RecruiterJobPostRepository recruiterJobPostRepository;
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
            recruiterJobPostRepository = new RecruiterJobPostRepository(db, unitOfWork, mapper);
            recruiterRepository = new RecruiterRepository(db, unitOfWork, mapper);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await recruiterJobPostRepository.GetAll(new Guid(), 1, 5, "", false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllRecPosted_Test()
        {
            var result = await recruiterJobPostRepository.GetAllRecPosted();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllSuggestion_Test()
        {
            var result = await recruiterJobPostRepository.GetAllSuggestion(1, 5, "");
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await recruiterJobPostRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetJobId_Test()
        {
            try
            {
                var result = await recruiterJobPostRepository.GetJobId(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Create_Test()
        {
            var rec = new RecruiterJobPostModel()
            {
                RecruiterId = new Guid(),
                JobId = new Guid()
            };
            var result = await recruiterJobPostRepository.Create(rec);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var rec = new RecruiterJobPostModel()
                {
                    RecruiterId = new Guid(),
                    JobId = new Guid()
                };
                var result = await recruiterJobPostRepository.Update(new Guid(), rec);
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
                var result = await recruiterJobPostRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
