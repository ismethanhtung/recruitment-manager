using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using InsternShip.UnitTest.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.UnitTest.Repository
{
    public class MailerRepositoryTests : BaseRepositoryClass
    {
        private MailerRepository mailerRepository;
        private readonly IConfiguration _config;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {

            var options = GetInMemoryDbOptions("InMemoryDatabase");
            var configuration = mockConfiguration();
            var db = new RecruitmentDB(options, configuration);
            unitOfWork = new UnitOfWork(db);
            mailerRepository = new MailerRepository(_config);
        }

        [Test]
        public async Task SendEmail_Test()
        {
            try
            {
                var mail = new MailRequestModel()
                {
                    To = "Test@gmail.com",
                    Subject = "Test",
                    Body = "Test"
                };
                var result = await mailerRepository.SendEmail(mail);
                Assert.AreEqual(true, result);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
