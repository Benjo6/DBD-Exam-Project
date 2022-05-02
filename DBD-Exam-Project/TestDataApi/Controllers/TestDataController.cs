using Microsoft.AspNetCore.Mvc;

namespace TestDataApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestDataController : ControllerBase
{
    
    private readonly ILogger<TestDataController> _logger;
    private readonly PrescriptionContext _pc;

    public TestDataController(ILogger<TestDataController> logger, PrescriptionContext pc)
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
