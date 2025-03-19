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

namespace InsternShip.UnitTest.Services
{
    public class JobServiceTests
    {
        private JobService jobService;

        [SetUp]
        public void Setup()
        {
            var jobview = new JobViewModel()
            {
                JobId = new Guid(),
                Name = "Test",
                Type = "Test",
                Level = "Test",
                Location = "Test",
                Description = "Test",
                Requirement = "Test",
                Benefit = "Test",
                MinSalary = 1000,
                MaxSalary = 2000,
                Quantity = 10,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                EndDate = DateTime.Now,
                JobStatus = true

            };

            var jobl = new List<JobViewModel>();
            jobl.Add(jobview);
            var joblist = new JobListViewModel()
            {
                TotalCount = 0,
                JobList = jobl
            };
            var mockIJobRepository = new Mock<IJobRepository>();
            mockIJobRepository.Setup(x => x.GetAll(1, 5, It.IsAny<string>())).Returns(Task.FromResult(joblist));
            mockIJobRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(jobview));
            mockIJobRepository.Setup(x => x.Create(It.IsAny<CreateJobModel>())).Returns(Task.FromResult(true));
            mockIJobRepository.Setup(x => x.CreateGUID(It.IsAny<CreateJobModel>())).Returns(Task.FromResult(new Guid()));
            mockIJobRepository.Setup(x => x.Update(It.IsAny<Guid>(),It.IsAny<JobUpdateModel>())).Returns(Task.FromResult(true));
            mockIJobRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            jobService = new JobService(mockIJobRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await jobService.GetAll(1,5, It.IsAny<string>());
            Assert.IsNotNull(result.JobList.ToList()[0].JobId);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Name);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Type);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Level);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Location);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Description);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Requirement);
            Assert.AreEqual("Test", result.JobList.ToList()[0].Benefit);
            Assert.AreEqual(1000, result.JobList.ToList()[0].MinSalary);
            Assert.AreEqual(2000, result.JobList.ToList()[0].MaxSalary);
            Assert.AreEqual(10, result.JobList.ToList()[0].Quantity);
            Assert.IsNotNull(result.JobList.ToList()[0].CreateDate);
            Assert.IsNotNull(result.JobList.ToList()[0].UpdateDate);
            Assert.IsNotNull(result.JobList.ToList()[0].EndDate);
            Assert.AreEqual(true, result.JobList.ToList()[0].JobStatus);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await jobService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.JobId);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual("Test", result.Type);
            Assert.AreEqual("Test", result.Level);
            Assert.AreEqual("Test", result.Location);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual("Test", result.Requirement);
            Assert.AreEqual("Test", result.Benefit);
            Assert.AreEqual(1000, result.MinSalary);
            Assert.AreEqual(2000, result.MaxSalary);
            Assert.AreEqual(10, result.Quantity);
            Assert.IsNotNull(result.CreateDate);
            Assert.IsNotNull(result.UpdateDate);
            Assert.IsNotNull(result.EndDate);
            Assert.AreEqual(true, result.JobStatus);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await jobService.Create(It.IsAny<CreateJobModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await jobService.Update(It.IsAny<Guid>(), It.IsAny<JobUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await jobService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
