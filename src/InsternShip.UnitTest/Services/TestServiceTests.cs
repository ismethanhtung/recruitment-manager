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
    public class TestServiceTests
    {
        private TestService testService;

        [SetUp]
        public void Setup()
        {
            var testview = new TestViewModel()
            {
                TestId = new Guid(),
                TotalScore = 0,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };
            var testviewl = new List<TestViewModel>();
            testviewl.Add(testview);

            var testlist = new TestListViewModel()
            {
                TotalCount = 0,
                TestList = testviewl
            };

            var mockITestRepository = new Mock<ITestRepository>();
            var mockIQuestionRepository = new Mock<IQuestionRepository>();
            var mockIQuestionBankRepository = new Mock<IQuestionBankRepository>();
            mockITestRepository.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(testlist));
            mockITestRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(testview));
            mockITestRepository.Setup(x => x.Create(It.IsAny<CreateTestModel>())).Returns(Task.FromResult(true));
            mockITestRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<TestUpdateModel>())).Returns(Task.FromResult(true));
            mockITestRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockITestRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            testService = new TestService(mockITestRepository.Object, mockIQuestionRepository.Object, mockIQuestionBankRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await testService.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TestList.ToList()[0].TestId);
            Assert.AreEqual(0, result.TestList.ToList()[0].TotalScore);
            Assert.IsNotNull(result.TestList.ToList()[0].StartTime);
            Assert.IsNotNull(result.TestList.ToList()[0].EndTime);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await testService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TestId);
            Assert.AreEqual(0, result.TotalScore);
            Assert.IsNotNull(result.StartTime);
            Assert.IsNotNull(result.EndTime);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await testService.Create(It.IsAny<CreateTestModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await testService.Update(It.IsAny<Guid>(), It.IsAny<TestUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await testService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await testService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
