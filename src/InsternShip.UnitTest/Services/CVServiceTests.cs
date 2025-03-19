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
using Microsoft.AspNetCore.Http;

namespace InsternShip.UnitTest.Services
{
    public class CVServiceTests
    {
        private CVService cvService;
        private IUnitOfWork unitOfWork;
        private IConfiguration configuration;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {

            var db = new RecruitmentDB();
            unitOfWork = new UnitOfWork(db);

            var cvmodel = new CVModel()
            {
                CandidateId = new Guid(),
                UrlFile = "Test",
                PublicIdFile = "Test"
            };

            var cv = new CV()
            {
                CVId = new Guid(),
                CandidateId = new Guid(),
                Candidate = new Candidate(),
                UrlFile = "Test",
                PublicIdFile = "Test"
            };
            var mockICVRepository = new Mock<ICVRepository>();
            var mockICloudinaryRepository = new Mock<ICloudinaryRepository>();
            mockICVRepository.Setup(x => x.GetByCanId(It.IsAny<Guid>())).Returns(Task.FromResult(cvmodel));
            mockICVRepository.Setup(x => x.Create(It.IsAny<CVModel>())).Returns(Task.FromResult(true));
            mockICVRepository.Setup(x => x.IsExistFile(It.IsAny<Guid>())).Returns(Task.FromResult(cv));
            mockICVRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            cvService = new CVService(mockICVRepository.Object, 
                                      mockICloudinaryRepository.Object);
        }

        [Test]
        public async Task Create_Test()
        {
            var createcv = new CreateCVModel()
            {
                CurrentId = new Guid(),
                File = null
            };
            var result = await cvService.Create(createcv);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }
    }
}
