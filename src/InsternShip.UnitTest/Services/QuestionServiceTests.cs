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
    public class QuestionServiceTests
    {
        private QuestionService questionService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var questioncreate = new CreateQuestionModel()
            {
                Detail = "Test",
                MaxScore = 0,
                Note = "Test"
            };

            var questionlist = new QuestionListViewModel()
            {
                TotalCount = 0
            };

            var question = new QuestionViewModel()
            {
                QuestionId = new Guid(),
                Detail = "Test",
                MaxScore = 0,
                Note = "Test",
                Tag = "Test",
                Level = 0
            };
            var mockIQuestionRepository = new Mock<IQuestionRepository>();
            var mockIQuestionBankRepository = new Mock<IQuestionBankRepository>();
            var mockITestRepository = new Mock<ITestRepository>();
            mockIQuestionRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(questionlist));
            mockIQuestionRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(question));
            mockIQuestionRepository.Setup(x => x.Create(It.IsAny<CreateQuestionModel>())).Returns(Task.FromResult(true));
            mockIQuestionRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<QuestionUpdateModel>())).Returns(Task.FromResult(true));
            mockIQuestionRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            questionService = new QuestionService(mockIQuestionRepository.Object,mockIQuestionBankRepository.Object, mockITestRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await questionService.GetAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TotalCount);
        }

        [Test]
        public async Task GetById()
        {
            var result = await questionService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.QuestionId);
            Assert.AreEqual("Test", result.Detail);
            Assert.AreEqual(0, result.MaxScore);
            Assert.AreEqual("Test", result.Note);
            Assert.AreEqual("Test", result.Tag);
            Assert.AreEqual(0, result.Level);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await questionService.Create(It.IsAny<CreateQuestionModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await questionService.Update(It.IsAny<Guid>(), It.IsAny<QuestionUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await questionService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
