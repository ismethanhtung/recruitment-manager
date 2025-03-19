using AutoMapper;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Api;
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
using Microsoft.AspNetCore.Mvc;

namespace InsternShip.UnitTest.Repository
{
    public class TestRepositoryTests : BaseRepositoryClass
    {
        private TestRepository testRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            testRepository = new TestRepository(db, unitOfWork, mapper);
        }

        public class TestController : Controller
        {
            private ITestRepository _repository;
            public TestController(ITestRepository repository)
            {
                _repository = repository;
            }

            public ActionResult Create(CreateTestModel request)
            {
                _repository.Create(request);

                return View();
            }
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await testRepository.GetAll(1, 5, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await testRepository.GetById(new Guid());
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
            var test = new CreateTestModel()
            {
                TotalScore = 10,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };
            var result = await testRepository.Create(test);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task CreateGUID_Test()
        {
            var test = new CreateTestModel()
            {
                TotalScore = 10,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };
            var result = await testRepository.CreateGUID(test);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var test = new TestUpdateModel()
                {
                    TotalScore = 10,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };
                var result = await testRepository.Update(new Guid(), test);
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
                var result = await testRepository.Delete(new Guid());
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
                Guid testid = (await testRepository.GetAll(1, 5, false)).TestList.ToList()[0].TestId;
                var result = await testRepository.Restore(testid);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
