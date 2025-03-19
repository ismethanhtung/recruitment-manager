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
    public class InterviewerServiceTests
    {
        private InterviewerService interviewerService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var interviewerview = new InterviewerViewModel()
            {
                InterviewerId = new Guid(),
                Description = "Test",
                UrlContact = "Test",
                IsDeleted = false
            };

            var interviewerL = new List<InterviewerViewModel>();
            interviewerL.Add(interviewerview);
            var interviewlist = new InterviewerListViewModel()
            {
                TotalCount = 0,
                InterviewerList = interviewerL
            };
            var mockIInterviewerRepository = new Mock<IInterviewerRepository>();
            var mockIAccountRepository = new Mock<IUserAccountRepository>();
            var mockIInterviewSessionRepository = new Mock<IInterviewSessionRepository>();
            var mockIUserInfoRepository = new Mock<IUserInfoRepository>();
            var mockIQuestionBankRepository = new Mock<IQuestionBankRepository>();
            mockIInterviewerRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(interviewlist));
            mockIInterviewerRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(interviewerview));
            mockIInterviewerRepository.Setup(x => x.Create(It.IsAny<CreateInterviewerModel>())).Returns(Task.FromResult(true));
            mockIInterviewerRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<InterviewerUpdateModel>())).Returns(Task.FromResult(true));
            mockIInterviewerRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIInterviewerRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            interviewerService = new InterviewerService(mockIInterviewerRepository.Object, 
                                                        mockIAccountRepository.Object,
                                                        mockIInterviewSessionRepository.Object,
                                                        mockIUserInfoRepository.Object,
                                                        mockIQuestionBankRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await interviewerService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
            Assert.IsNotNull(result.InterviewerList.ToList()[0].InterviewerId);
            Assert.AreEqual("Test", result.InterviewerList.ToList()[0].Description);
            Assert.AreEqual("Test", result.InterviewerList.ToList()[0].UrlContact);
            Assert.AreEqual(false, result.InterviewerList.ToList()[0].IsDeleted);
        }

        [Test]
        public async Task GetById()
        {
            var result = await interviewerService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.InterviewerId);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual("Test", result.UrlContact);
            Assert.AreEqual(false, result.IsDeleted);
        }

        [Test]
        public async Task Create_Test()
        {
            var interviewercreat = new CreateInterviewerModel()
            {
                UserId = new Guid()
            };
            var result = await interviewerService.Create(interviewercreat);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            try
            {
                var interviewer = new InterviewerUserInfoUpdateModel();
                var result = await interviewerService.Update(new Guid(), interviewer);
                Assert.IsNotNull(result);
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
            var result = await interviewerService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await interviewerService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
