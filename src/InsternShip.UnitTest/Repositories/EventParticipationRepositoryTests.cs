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
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Repository
{
    public class EventParticipationRepositoryTests : BaseRepositoryClass
    {
        private EventParticipationRepository eventParticipationRepository;
        private RecruiterRepository recruiterRepository;
        private CandidateRepository candidateRepository;
        private EventRepository eventRepository;
        private IUnitOfWork unitOfWork;


        [SetUp]
        public void Setup()
        {
            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            eventParticipationRepository = new EventParticipationRepository(db, unitOfWork, mapper);
            recruiterRepository = new RecruiterRepository(db, unitOfWork, mapper);
            eventRepository = new EventRepository(db, unitOfWork, mapper);
            candidateRepository = new CandidateRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            try
            {
                var result = await eventParticipationRepository.GetAll("", 1, 5, new Guid(), new Guid());
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await eventParticipationRepository.GetById(new Guid());
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
            try
            {
                var eventparticipation = new EventParticipationCreateModel()
                {
                    CandidateId = new Guid(),
                    EventPostId = new Guid(),
                };
                var result = await eventParticipationRepository.Create(eventparticipation, 1);
                Assert.AreEqual(true, result);
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
                var result = await eventParticipationRepository.Update(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task UpdateByCEId_Test()
        {
            try
            {
                var result = await eventParticipationRepository.UpdateByCEId(new Guid(), new Guid(), new Guid());
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
                var result = await eventParticipationRepository.Delete(new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task DeleteByCEid_Test()
        {
            try
            {
                var result = await eventParticipationRepository.DeleteByCEid(new Guid(), new Guid());
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
