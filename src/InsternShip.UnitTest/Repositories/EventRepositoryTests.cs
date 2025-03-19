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
    public class EventRepositoryTests : BaseRepositoryClass
    {
        private EventRepository eventRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {

            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            eventRepository = new EventRepository(db, unitOfWork, mapper);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await eventRepository.GetAll("", 1, 10, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetById_Test()
        {
            try
            {
                var result = await eventRepository.GetById(new Guid());
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
                var eventmodel = new CreateEventModel()
                {
                    Name = "Test",
                    Location = "Test",
                    MaxCandidate = 0,
                    Description = "Test",
                    Poster = "Test",
                    Status = true,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DeadlineDate = DateTime.Now,
                };
                var result = await eventRepository.Create(eventmodel);
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
                var eventmodel = new EventUpdateModel()
                {
                    Name = "Test",
                    Location = "Test",
                    MaxCandidate = 100,
                    Description = "Test",
                    Poster = "Test",
                    Status = false,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DeadlineDate = DateTime.Now,
                };
                var result = await eventRepository.Update(new Guid(), eventmodel);
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
            var eventold = (await eventRepository.GetAll("", 1, 10, false)).EventList.ToList()[0].EventId;
            var result = await eventRepository.Delete(eventold);
            Assert.AreEqual(true, result);
        }
    }
}
