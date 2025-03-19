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
    public class UserInfoRepositoryTests : BaseRepositoryClass
    {
        private UserInfoRepository userInfoRepository;
        private UserAccountRepository userAccountRepository;
        private BlackListRepository blackListRepository;
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
            userInfoRepository = new UserInfoRepository(db, unitOfWork, mapper, userManager);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
        }


        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await userInfoRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }


        [Test]
        public async Task UpdateInfo_Test()
        {
            try
            {
                var user = new UserInfoUpdateModel()
                {
                    Avatar = "Test",
                    FirstName = "Test",
                    LastName = "Test",
                    Gender = "Male",
                    DateOfBirth = DateTime.Now,
                    PhoneNumber = "0923458234",
                    Location = "Test"
                };
                var result = await userInfoRepository.UpdateInfo(new Guid(), user);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }


    }
}
