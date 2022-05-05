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
        private readonly PrescriptionContext _pc;

        public TestDataController(ILogger<TestDataController> logger,PrescriptionContext pc)
        {
            _logger = logger;
            _pc = pc;

        }

        [HttpGet("SeedTestData")]
        public Task Seed()
        {
            new DbSeeder(_pc).SeedTestData(1);
            return Task.CompletedTask;
        }

        [HttpGet("SeedTestDataReduced")]
        public Task SeedReduced()
        {
            new DbSeeder(_pc).SeedTestData(100);
            return Task.CompletedTask;
        }
    }
}
