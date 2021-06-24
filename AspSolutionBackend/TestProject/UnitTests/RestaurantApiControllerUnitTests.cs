using System;
using System.Collections;
using System.Collections.Generic;
using DAL.APP.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TestProject.UnitTests
{
    //ARRANGE - COMMON
    public class RestaurantApiControllerUnitTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        // private readonly IAppBll _bll;
        private readonly AppDbContext _ctx;
        
        public RestaurantApiControllerUnitTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //provide new random database name here
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _ctx = new AppDbContext(optionBuilder.Options);
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();

            // _testController = new TestController
            // using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            // var logger = loggerFactory.CreateLogger<TestController>()

            // SUT
            //_testController = new TestController(logger, _ctx);
        }


        private void SeedData()
        {
        }
    }

    public class TestGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>()
        {
            new object[] {1},
            new object[] {5},
            new object[] {100}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 
    }
}