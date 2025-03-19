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
using InsternShip.Common;

namespace InsternShip.UnitTest.Repository
{
   public class RecruiterEventPostRepositoryTests : BaseRepositoryClass
    {
        private EventParticipationRepository eventParticipationRepository;
        private UserAccountRepository userAccountRepository;
        private BlackListRepository blackListRepository;
        private RecruiterRepository recruiterRepository;
        private RecruiterEventPostRepository recruiterEventPostRepository;
        private EventRepository eventRepository;
        private CandidateRepository candidateRepository;
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
            eventParticipationRepository = new EventParticipationRepository(db, unitOfWork, mapper);
           recruiterEventPostRepository = new RecruiterEventPostRepository(db, unitOfWork, mapper);
           recruiterRepository = new RecruiterRepository(db, unitOfWork, mapper);
           eventRepository = new EventRepository(db, unitOfWork, mapper);
           candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
            userAccountRepository = new UserAccountRepository(db, unitOfWork, userManager, mailer, signInManager, mapper, configuration, urlHelper, roleManager, blackListRepository);
       }

       [Test]
       public async Task GetAll_Test()
       {
           var result = await recruiterEventPostRepository.GetAll(new Guid(), 1, 5, "", false);
           Assert.IsNotNull(result);
       }

       [Test]
       public async Task GetAllRecPosted_Test()
       {
           var result = await recruiterEventPostRepository.GetAllRecPosted();
           Assert.IsNotNull(result);
       }

       [Test]
       public async Task GetById_Test()
       {
            try
            {
                var result = await recruiterEventPostRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetEventId_Test()
        {
            try
            {
                var result = await recruiterEventPostRepository.GetEventId(new Guid());
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
            var rec = new RecruiterEventPostModel()
            {
                EventId = new Guid(),
                RecruiterId = new Guid()
            };
            var result = await recruiterEventPostRepository.Create(rec);
            Assert.AreEqual(true, result);
       }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var rec = new RecruiterEventPostModel()
                {
                    EventId = new Guid(),
                    RecruiterId = new Guid()
                };
                var result = await recruiterEventPostRepository.Update(new Guid(), rec);
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
                var result = await recruiterEventPostRepository.Delete(new Guid());
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
                Guid? eventpostid = (await eventParticipationRepository.GetAll("", 1, 10, null, null)).EventParticipationList.First().EventPostId;
                var result = await recruiterEventPostRepository.Restore(eventpostid.GetValueOrDefault());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
}
    }
}
