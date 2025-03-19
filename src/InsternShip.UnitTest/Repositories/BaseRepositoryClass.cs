using AutoMapper;
using InsternShip.Data;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.Repositories;
using InsternShip.Data.ViewModels;
using InsternShip.Service;
using InsternShip.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InsternShip.UnitTest.Repositories
{
    public class BaseRepositoryClass
    {
        protected IMapper mapper { get; private set; }

        [OneTimeSetUp]
        public void SetupBase()
        {
            // Initialize the IMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>(); // Add the profile you created for AutoMapper
                // Register other AutoMapper profiles if needed
            });

            mapper = mapperConfig.CreateMapper();
        }

        protected DbContextOptions<RecruitmentDB> GetInMemoryDbOptions(string dbName)
        {

            var options = new DbContextOptionsBuilder<RecruitmentDB>()
                .UseInMemoryDatabase(databaseName: dbName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return options;
        }

        protected IConfiguration mockConfiguration()
        {

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value)
                         .Returns($"Data Source=HARUNA\\SQLEXPRESS;Initial Catalog=RecruitmentDB;Integrated Security=True;Encrypt=False");
            return configuration.Object;
        }
    }
}
