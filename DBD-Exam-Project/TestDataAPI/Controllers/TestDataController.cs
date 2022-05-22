using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDataAPI.Context;
using TestDataAPI.Seeder;

namespace TestDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDataController : ControllerBase
    {

        private readonly ILogger<TestDataController> _logger;
        private readonly DbSeeder _dbSeeder;
        private readonly PrescriptionContext _pc;

        public TestDataController(ILogger<TestDataController> logger, DbSeeder dbSeeder)
        {
            _logger = logger;
            _dbSeeder = dbSeeder;

        }

        [HttpGet("SeedTestData")]
        public Task Seed()
        {
            _dbSeeder.SeedTestData(1);
            return Task.CompletedTask;
        }

        [HttpGet("SeedTestDataReduced")]
        public Task SeedReduced()
        {
            _dbSeeder.SeedTestData(100);
            return Task.CompletedTask;
        }
    }
}
