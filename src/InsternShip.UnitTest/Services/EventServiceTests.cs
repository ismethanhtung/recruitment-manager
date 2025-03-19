using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Services
{
    public class EventServiceTests
    {
        private EventService eventService;

        [SetUp]
        public void Setup()
        {
            var eventt = new EventViewModel()
            {
                EventId = new Guid(),
                Name = "Test",
                Location = "Test",
                MaxCandidate = 0,
                Description = "Test",
                Poster = "Test",
                Status = true,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                PostDate = DateTime.Now,
                DeadlineDate = DateTime.Now
            };

            var eventl = new List<EventViewModel>();
            eventl.Add(eventt);
            var eventlist = new EventListViewModel()
            {
                TotalCount = 0,
                EventList = eventl
            };
            var mockIEventRepository = new Mock<IEventRepository>();
            mockIEventRepository.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(eventlist));
            mockIEventRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(eventt));
            mockIEventRepository.Setup(x => x.Create(It.IsAny<CreateEventModel>())).Returns(Task.FromResult(true));
            mockIEventRepository.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<EventUpdateModel>())).Returns(Task.FromResult(true));
            mockIEventRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockIEventRepository.Setup(x => x.Restore(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            eventService = new EventService(mockIEventRepository.Object);
        }

        [Test]
        public async Task GetAll_Test()
        {
            var result = await eventService.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            Assert.IsNotNull(result.EventList.ToList()[0].EventId);
            Assert.AreEqual("Test", result.EventList.ToList()[0].Name);
            Assert.AreEqual("Test", result.EventList.ToList()[0].Location);
            Assert.AreEqual(0, result.EventList.ToList()[0].MaxCandidate);
            Assert.AreEqual("Test", result.EventList.ToList()[0].Description);
            Assert.AreEqual("Test", result.EventList.ToList()[0].Poster);
            Assert.AreEqual(true, result.EventList.ToList()[0].Status);
            Assert.IsNotNull(result.EventList.ToList()[0].StartDate);
            Assert.IsNotNull(result.EventList.ToList()[0].EndDate);
            Assert.IsNotNull(result.EventList.ToList()[0].PostDate);
            Assert.IsNotNull(result.EventList.ToList()[0].DeadlineDate);
        }

        [Test]
        public async Task GetById_Test()
        {
            var result = await eventService.GetById(It.IsAny<Guid>());
            Assert.IsNotNull(result.EventId);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual("Test", result.Location);
            Assert.AreEqual(0, result.MaxCandidate);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual("Test", result.Poster);
            Assert.IsNotNull(result.Status);
            Assert.IsNotNull(result.StartDate);
            Assert.IsNotNull(result.EndDate);
            Assert.IsNotNull(result.PostDate);
            Assert.IsNotNull(result.DeadlineDate);
        }

        [Test]
        public async Task Create_Test()
        {
            var result = await eventService.Create(It.IsAny<CreateEventModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Update_Test()
        {
            var result = await eventService.Update(It.IsAny<Guid>(), It.IsAny<EventUpdateModel>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Delete_Test()
        {
            var result = await eventService.Delete(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Restore_Test()
        {
            var result = await eventService.Restore(It.IsAny<Guid>());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
