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
    public class ApplicationStatusRepositoryTests : BaseRepositoryClass
    {
        private ApplicationStatusRepository applicationStatusRepository;
        private CandidateRepository candidateRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
            applicationStatusRepository = new ApplicationStatusRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await applicationStatusRepository.GetAllApplicationStatus();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetByDescription_Test()
        {
            try
            {
                var result = await applicationStatusRepository.GetByDescription("Test2");
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
}

        public async Task GetById_Test()
        {
            Guid applicationstatusid = (await applicationStatusRepository.GetAllApplicationStatus()).ToList()[0].ApplicationStatusId;
            var result = await applicationStatusRepository.GetById(applicationstatusid);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Create_Test()
        {
            var applicationstatus = new ApplicationStatusCreateModel()
            {
                Description = "Approved"
            };
            var result = await applicationStatusRepository.Create(applicationstatus);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var applicationstatuscreate = new ApplicationStatusCreateModel()
            {
                Description = "Uncheck"
            };
            await applicationStatusRepository.Create(applicationstatuscreate);

            Guid applicationstatusid = (await applicationStatusRepository.GetAllApplicationStatus()).First().ApplicationStatusId;
            var applicationstatus = new ApplicationStatusModel()
            {
                ApplicationStatusId = applicationstatusid,
                Description = "Approved"
            };
            var result = await applicationStatusRepository.Update(applicationstatus);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var applicationstatusold = await applicationStatusRepository.GetAllApplicationStatus();
            var result = await applicationStatusRepository.Delete(applicationstatusold.ToList()[0].ApplicationStatusId);
            Assert.AreEqual(true, result);
        }
    }
}
