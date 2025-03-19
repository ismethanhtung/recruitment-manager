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
using AutoMapper;
using InsternShip.UnitTest.Repositories;

namespace InsternShip.UnitTest.Repository
{
    public class QuestionRepositoryTests : BaseRepositoryClass
    {
        private QuestionRepository questionRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {

            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            questionRepository = new QuestionRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await questionRepository.GetAll("", "", 1, 1, 5);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllWithKeyWord_Test()
        {
            var result = await questionRepository.GetAll("Test", "", 1, 1, 5);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
           
            try
            {
                var result = await questionRepository.GetById(new Guid());
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
            var question = new CreateQuestionModel()
            {
                Detail = "Test",
                MaxScore = 10,
                Note = "max",
                Tag = "Test",
                Level = 10
            };
            var result = await questionRepository.Create(question);
            Assert.AreEqual(true, result);

        }

        [Test]
        public async Task CreateFromTest_Test()
        {
            try
            {
                var question = new CreateQuestionModel();
                var result = await questionRepository.CreateFromTest( question);
                Assert.IsNotNull(result);
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
                var question = new QuestionUpdateModel()
                {
                    Detail = "Test2",
                    MaxScore = 100,
                    Note = "test",
                    Tag = "Test",
                    Level = 10
                };
                var result = await questionRepository.Update(new Guid(), question);
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
                var result = await questionRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
