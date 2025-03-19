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
using Microsoft.Extensions.Configuration;

namespace InsternShip.UnitTest.Repository
{
    public class JobRepositoryTests : BaseRepositoryClass
    {
        private JobRepository jobRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            jobRepository = new JobRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await jobRepository.GetAll(1, 5, "");
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllWithKeyWord_Test()
        {
            var result = await jobRepository.GetAll(1, 5, "test");
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Create_Test()
        {
            var job = new CreateJobModel()
            {
                Type = "Test",
                Level = "Test",
                Name = "Test",
                Location = "Test",
                Description = "Test",
                Requirement = "Test",
                Benefit = "Test",
                MinSalary = 1000,
                MaxSalary = 2000,
                Quantity = 10,
                // CreateDate = DateTime.Now,
                // UpdateDate = DateTime.Now,
                EndDate = DateTime.Now,
                JobStatus = true,
                // IsDeleted = false
            };
            var result = await jobRepository.Create(job);
            Assert.AreEqual(true, result);

        }

        [Test]
        public async Task Update_Test()
        {
            Guid jobid = (await jobRepository.GetAll(1, 1, "")).JobList.ToList()[0].JobId;
            var job = new JobUpdateModel()
            {
                Type = "Test",
                Level = "Test",
                Name = "Test",
                Location = "Test",
                Description = "Test",
                Requirement = "Test",
                Benefit = "Test",
                MinSalary = 1000,
                MaxSalary = 2000,
                Quantity = 10,
                CreateDate = DateTime.Now,
                EndDate = DateTime.Now,
                JobStatus = true,
            };
            var result = await jobRepository.Update(jobid, job);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var jobold = await jobRepository.GetAll(1, 1, "");
            var result = await jobRepository.Delete(jobold.JobList.ToList()[0].JobId);
            Assert.AreEqual(true, result);
        }
    }
}
