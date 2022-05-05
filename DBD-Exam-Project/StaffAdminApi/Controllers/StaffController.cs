using Microsoft.AspNetCore.Mvc;

namespace StaffAdminApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly ILogger<StaffController> _logger;

    public StaffController(ILogger<StaffController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "PostStaff")]
    public IEnumerable<Staff> Create()
    {
        return null;
    }

    [HttpGet(Name = "GetAllStaff")]
    public IEnumerable<Staff> ReadAll()
    {
        return null;
    }

    [HttpGet(Name = "GetStaff")]
    public IEnumerable<Staff> Read(string id)
    {
        return null;
    }

    [HttpGet(Name = "UpdateStaff")]
    public IEnumerable<Staff> Update()
    {
        return null;
    }

    [HttpDelete(Name = "DeleteStaff")]
    public IEnumerable<Staff> Delete()
    {
        return null;
    }
}
