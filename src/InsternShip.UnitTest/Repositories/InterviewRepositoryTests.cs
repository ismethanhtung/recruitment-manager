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
    public class InterviewRepositoryTests : BaseRepositoryClass
    {
        private InterviewRepository interviewRepository;
        private ApplicationRepository applicationRepository;
        private RecruiterRepository recruiterRepository;
        private CandidateRepository candidateRepository;
        private RecruiterJobPostRepository recruiterJobPostRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            interviewRepository = new InterviewRepository(db, unitOfWork, mapper);
            applicationRepository = new ApplicationRepository(db, unitOfWork, mapper);
            recruiterRepository = new RecruiterRepository(db, unitOfWork, mapper);
            recruiterJobPostRepository = new RecruiterJobPostRepository(db, unitOfWork, mapper);
            candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await interviewRepository.GetAll(1, 5, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllInterviewInWeek_Test()
        {
            var result = await interviewRepository.GetAllInterviewInWeek();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllInWeekByRecId_Test()
        {
            try
            {
                var result = await interviewRepository.GetAllInWeekByRecId(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetScheduleInterviewByRecruiter_Test()
        {
            try
            {
                var result = await interviewRepository.GetScheduleInterviewByRecruiter(new Guid());
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
                var result = await interviewRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetByApplicationId_Test()
        {
            try
            {
                var result = await interviewRepository.GetByApplicationId(new Guid());
                Assert.IsNull(result);
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
                Guid applicationid = (await applicationRepository.GetAll("", 1, 5, "", null, null, false)).ApplicationList.ToList()[0].ApplicationId;

                var interview = new CreateInterviewModel()
                {
                    ApplicationId = applicationid,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Location = "Test",
                };
                var result = await interviewRepository.Create(interview);
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
                var interview = new InterviewUpdateModel()
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Location = "Test",
                };
                var result = await interviewRepository.Update(new Guid(), interview);
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
                var result = await interviewRepository.Delete(new Guid());
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
                Guid interviewid = (await interviewRepository.GetAll(1, 1, false)).InterviewList.ToList()[0].InterviewId;
                var result = await interviewRepository.Restore(interviewid);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
