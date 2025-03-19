using AutoMapper;
using Microsoft.Extensions.Configuration;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class QuestionBankServiceTests
    {
        private QuestionBankService questionbankService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var questionbankcreate = new QuestionBankViewModel()
            {
                TestId = new Guid(),
                QuestionId = new Guid()
            };

            var questionbankview = new List<QuestionBankViewModel>()
            {
                   new QuestionBankViewModel()
                {
                   TestId = new Guid(),
                   QuestionId = new Guid()
                }
            };

            IEnumerable<QuestionBankViewModel> questionbankmock = questionbankview;
            var mockIQuestionBankRepository = new Mock<IQuestionBankRepository>();
            mockIQuestionBankRepository.Setup(x => x.GetAll(It.IsAny<string>())).Returns(Task.FromResult(questionbankmock));
            mockIQuestionBankRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(Task.FromResult(questionbankcreate));
            mockIQuestionBankRepository.Setup(x => x.Create(It.IsAny<CreateQuestionBankModel>())).Returns(Task.FromResult(true));
            mockIQuestionBankRepository.Setup(x => x.Update(It.IsAny<QuestionBankModel>())).Returns(Task.FromResult(true));
            mockIQuestionBankRepository.Setup(x => x.Delete(It.IsAny<string>())).Returns(Task.FromResult(true));
            questionbankService = new QuestionBankService(mockIQuestionBankRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await questionbankService.GetAll(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToList()[0].TestId);
            Assert.IsNotNull(result.ToList()[0].QuestionId);
        }

        [Test]
        public async Task GetById()
        {
            var result = await questionbankService.GetById(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TestId);
            Assert.IsNotNull(result.QuestionId);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await questionbankService.Create(It.IsAny<CreateQuestionBankModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await questionbankService.Update(It.IsAny<QuestionBankModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await questionbankService.Delete(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
