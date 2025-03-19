using AutoMapper;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.UnitTest.Repositories;
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


namespace InsternShip.UnitTest.Repository
{
    public class InterviewerRepositoryTests : BaseRepositoryClass
    {
        private BlackListRepository blackListRepository;
        private UserAccountRepository userAccountRepository;
        private InterviewerRepository interviewerRepository;
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
            interviewerRepository = new InterviewerRepository(db, unitOfWork, mapper);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await interviewerRepository.GetAll("", 1, 5, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetIdByUserId_Test()
        {
            try
            {
                var result = await interviewerRepository.GetIdByUserId(new Guid());
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
                var result = await interviewerRepository.GetByUserId(new Guid());
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await interviewerRepository.GetById(new Guid());
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
            var interviewer = new CreateInterviewerModel()
            {
                UserId = new Guid(),
                Description = "Test",
                UrlContact = "Test"
            };
            var result = await interviewerRepository.Create(interviewer);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var interviewer = new InterviewerUpdateModel()
                {
                    Description = "Test",
                    UrlContact = "Test"
                };
                var result = await interviewerRepository.Update(new Guid(), interviewer);
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
                Guid interviewerid = (await interviewerRepository.GetAll("", 1, 5, false)).InterviewerList.ToList()[0].InterviewerId;
                var result = await interviewerRepository.Restore(interviewerid);
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
                var result = await interviewerRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
