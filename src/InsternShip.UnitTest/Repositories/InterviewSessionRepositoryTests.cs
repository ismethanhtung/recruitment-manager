using AutoMapper;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using InsternShip.UnitTest.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Repository
{
    public class InterviewSessionRepositoryTests : BaseRepositoryClass
    {
        private InterviewSessionRepository interviewSessionRepository;
        private InterviewerRepository interviewerRepository;
        private InterviewRepository interviewRepository;
        private ApplicationRepository applicationRepository;
        private TestRepository testRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            interviewSessionRepository = new InterviewSessionRepository(db, unitOfWork, mapper);
            interviewerRepository = new InterviewerRepository(db, unitOfWork, mapper);
            interviewRepository = new InterviewRepository(db, unitOfWork, mapper);
            applicationRepository = new ApplicationRepository(db, unitOfWork, mapper);
            testRepository = new TestRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAllInterviewSession_Test()
        {
            try
            {
                var result = await interviewSessionRepository.GetAllInterviewSession(new Guid(), 1, 5);
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetAllInterviewSession2_Test()
        {
            
            try
            {
                var result = await interviewSessionRepository.GetAllInterviewSession(new Guid());
                Assert.IsNotNull(result);
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
                var result = await interviewSessionRepository.GetById(new Guid());
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
            try
            {
                var interviewss = new CreateInterviewSessionModel()
                {
                    Note = "Test",
                    InterviewId = new Guid(),
                    InterviewerId = new Guid(),
                    TestId = new Guid()
                };
                var result = await interviewSessionRepository.Create(interviewss);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var interviewss = new UpdatedInterviewSessionModel()
                {
                    GivenScore = 10,
                    Note = "Test",
                };
                var result = await interviewSessionRepository.Update(new Guid(), interviewss);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task SaveScore_Test()
        {
            try
            {
                var interviewss = new UpdatedInterviewSessionModel()
                {
                    GivenScore = 11,
                    Note = "Test",
                };
                var result = await interviewSessionRepository.SaveScore(new Guid(), new Guid(), interviewss);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetAllSessionOfInterview_Test()
        {
            var result = await interviewSessionRepository.GetAllSessionOfInterview(new Guid());
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Delete_Test()
        {
            try
            {
                var result = await interviewSessionRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetTestId_Test()
        {
            
            try
            {
                var result = await interviewSessionRepository.GetTestId(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task CheckDuplicate_Test()
        {
            var result = await interviewSessionRepository.CheckDuplicate(new Guid(), DateTime.Now, DateTime.Now);
            Assert.IsNotNull(result);
        }
    }
}
