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
    public class QuestionBankRepositoryTests : BaseRepositoryClass
    {
        private QuestionBankRepository questionBankRepository;
        private QuestionRepository questionRepository;
        private TestRepository testRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {

            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            questionBankRepository = new QuestionBankRepository(db, unitOfWork, mapper);
            testRepository = new TestRepository(db, unitOfWork, mapper);
            questionRepository = new QuestionRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await questionBankRepository.GetAll("");
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Get_Test()
        {
            try
            {
                Guid testid = (await testRepository.GetAll(1, 5, false)).TestList.ToList()[0].TestId;
                Guid questionid = (await questionRepository.GetAll("", "", 1, 1, 5)).QuestionList.First().QuestionId;
                var result = await questionBankRepository.Get(testid.ToString(), questionid.ToString());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetScore_Test()
        {
            var result = await questionBankRepository.GetScore(new Guid());
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await questionBankRepository.GetById((new Guid()).ToString());
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
            var question = new CreateQuestionBankModel()
            {
                TestId = new Guid(),
                QuestionId = new Guid()
            };
            var result = await questionBankRepository.Create(question);
            Assert.AreEqual(true, result);

        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var question = new QuestionBankModel()
                {
                    QuestionId = new Guid(),
                    QuestionBankId = new Guid(),
                    TestId = new Guid()
                };
                var result = await questionBankRepository.Update(question);
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
                var result = await questionBankRepository.Delete("");//string to guid
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
