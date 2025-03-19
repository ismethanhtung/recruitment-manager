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
    public class ApplicationRepositoryTests : BaseRepositoryClass
    {
        private ApplicationRepository applicationRepository;
        private ApplicationStatusUpdateRepository applicationStatusUpdateRepository;
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
            recruiterJobPostRepository = new RecruiterJobPostRepository(db, unitOfWork, mapper);
            candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
            applicationRepository = new ApplicationRepository(db, unitOfWork, mapper);
            applicationStatusUpdateRepository = new ApplicationStatusUpdateRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await applicationRepository.GetAll("", 1, 5, "", null, null, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetMailInfoByAppId_Test()
        {
             var result = await applicationRepository.GetMailInfoByAppId(new Guid());
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await applicationRepository.GetById(new Guid());
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
            var application = new ApplicationCreateModel()
            {
                CandidateId = new Guid(),
                JobPostId = new Guid()
            };
            var result = await applicationRepository.Create(application);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateGuid_Test()
        {
            try
            {
                var application = new ApplicationCreateModel()
                {
                    CandidateId = new Guid(),
                    JobPostId = new Guid()
                };
                var result = await applicationRepository.CreateGuid(application);
                Assert.IsNotNull(result);

            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task AlreadyAppliedForJob_Test()
        {
            var application = new ApplicationModel()
            {
                CandidateId = new Guid(),
                JobPostId = new Guid(),
                ApplicationId = new Guid()
            };
            var result = await applicationRepository.AlreadyAppliedForJob(application);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Delete_Test()
        {
            try
            {
                var application = new ApplicationCreateModel()
                {
                    CandidateId = new Guid(),
                    JobPostId = new Guid()
                };
                await applicationRepository.Create(application);
                var applicationold = await applicationRepository.GetAll("", 1, 5, "", application.CandidateId, application.JobPostId, false);
                var result = await applicationRepository.Delete(applicationold.ApplicationList.First().ApplicationId);
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
                var application = await applicationRepository.GetAll("", 1, 5, "", null, null, false);
                var result = await applicationRepository.Restore(application.ApplicationList.ToList()[0].ApplicationId);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
