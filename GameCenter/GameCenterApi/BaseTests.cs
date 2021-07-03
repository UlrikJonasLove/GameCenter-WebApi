using AutoMapper;
using GameCenter.Data;
using GameCenter.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenter.Tests
{
    public class BaseTests
    {
        protected AppDbContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new AppDbContext(options);
            return dbContext;
        }

        protected IMapper BuildMap()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }
    }
}
