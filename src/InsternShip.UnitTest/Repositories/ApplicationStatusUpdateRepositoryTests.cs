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
using static System.Net.Mime.MediaTypeNames;

namespace InsternShip.UnitTest.Repository
{
    public class ApplicationStatusUpdateRepositoryTests : BaseRepositoryClass
    {
        private ApplicationStatusUpdateRepository applicationStatusUpdateRepository;
        private ApplicationRepository applicationRepository;
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
            applicationStatusUpdateRepository = new ApplicationStatusUpdateRepository(db, unitOfWork, mapper);
            applicationRepository = new ApplicationRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAllApplicationStatusUpdate_Test()
        {
            var result = await applicationStatusUpdateRepository.GetAllApplicationStatusUpdate();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllByApplicationId_Test()
        {
            var applicationstatus = new ApplicationStatusUpdateCreateModel()
            {
                ApplicationId = new Guid(),
                StatusId = new Guid()
            };
            await applicationStatusUpdateRepository.Create(applicationstatus);
            var result = await applicationStatusUpdateRepository.GetAllByApplicationId(applicationstatus.ApplicationId);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetByApplicationId_Test()
        {
            try
            {
                var applicationstatus = new ApplicationStatusUpdateCreateModel()
                {
                    ApplicationId = new Guid(),
                    StatusId = new Guid()
                };
                await applicationStatusUpdateRepository.Create(applicationstatus);
                var applicationid = await applicationStatusUpdateRepository.GetAllApplicationStatusUpdate();
                var result = await applicationStatusUpdateRepository.GetByApplicationId(applicationid.First().ApplicationId);
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
                var result = await applicationStatusUpdateRepository.GetById(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task AcceptApplication_Test()
        {
            try
            {
                var applicationstatus = new ApplicationStatusUpdateCreateModel()
                {
                    ApplicationId = new Guid(),
                    StatusId = new Guid()
                };
                var result = await applicationStatusUpdateRepository.AcceptApplication(new Guid(), new Guid(), new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task RejectApplication_Test()
        {
            try
            {
                var applicationstatus = new ApplicationStatusUpdateCreateModel()
                {
                    ApplicationId = new Guid(),
                    StatusId = new Guid()
                };
                var result = await applicationStatusUpdateRepository.RejectApplication(new Guid(), new Guid(), new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Create_Test()
        {
            var applicationstatus = new ApplicationStatusUpdateCreateModel()
            {
                ApplicationId = new Guid(),
                StatusId = new Guid()
            };
            var result = await applicationStatusUpdateRepository.Create(applicationstatus);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            Guid application = (await applicationStatusUpdateRepository.GetAllApplicationStatusUpdate()).ToList()[0].ApplicationId;

            var applicationstatus = new ApplicationStatusUpdateModel()
            {
                ApplicationId = application,
                ApplicationStatusUpdateId = new Guid(),
                LatestUpdate = DateTime.Now,
                StatusId = new Guid()
            };
            var result = await applicationStatusUpdateRepository.Update(applicationstatus);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            try
            {
                var result = await applicationStatusUpdateRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
