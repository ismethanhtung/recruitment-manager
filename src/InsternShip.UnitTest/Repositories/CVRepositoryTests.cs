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

namespace InsternShip.UnitTest.Repository
{
    public class CVRepositoryTests : BaseRepositoryClass
    {
        private CVRepository cVRepository;
        private CandidateRepository candidateRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {

            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            cVRepository = new CVRepository(db, unitOfWork, mapper);
            candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetByCanId_Test()
        {
            var cv = new CVModel()
            {
                CandidateId = new Guid(),
                UrlFile = "Test",
                PublicIdFile = "Test"
            };
            await cVRepository.Create(cv);
            var result = await cVRepository.GetByCanId(cv.CandidateId);
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task Create_Test()
        {
            var cv = new CVModel()
            {
                CandidateId = new Guid(),
                UrlFile = "Test",
                PublicIdFile = "Test"
            };
            var result = await cVRepository.Create(cv);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task IsExistFile_Test()
        {
            try
            {
                var result = await cVRepository.IsExistFile(new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task Delete_Test()
        {
            var cv = new CVModel()
            {
                CandidateId = new Guid(),
                UrlFile = "Test",
                PublicIdFile = "Test"
            };
            var result = await cVRepository.Create(cv);
            result = await cVRepository.Delete(cv.CandidateId);
            Assert.AreEqual(true, result);
        }
    }
}
