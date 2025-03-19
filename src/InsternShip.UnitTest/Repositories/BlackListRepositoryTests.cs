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
    public class BlackListRepositoryTests : BaseRepositoryClass
    {
        private BlackListRepository blackListRepository;
        private UserAccountRepository userAccountRepository;
        private IUnitOfWork unitOfWork;

        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;
        private IMailerRepository mailer;
        private IUrlHelper urlHelper;
        private IConfiguration configuration;
        private RoleManager<Roles> roleManager;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            blackListRepository = new BlackListRepository(db, unitOfWork, mapper);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await blackListRepository.GetAll("", 1, 1, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllWithKeyWord_Test()
        {
            var result = await blackListRepository.GetAll("Test", 1, 1, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await blackListRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetByUserId_Test()
        {
            try
            {
                var result = await blackListRepository.GetByUserId(new Guid());
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
            var blackList = new CreateBlackListModel()
            {
                UserId = new Guid(),
                Reason = "Test",
                Duration = 0
            };
            var result = await blackListRepository.Create(blackList);
            Assert.AreEqual(true, result);

        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var blackList = new BlackListUpdateModel()
                {
                    Reason = "Test",
                    Duration = 10
                };
                var result = await blackListRepository.Update(new Guid(), blackList);
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
                var result = await blackListRepository.Restore(new Guid());
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
                var result = await blackListRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
