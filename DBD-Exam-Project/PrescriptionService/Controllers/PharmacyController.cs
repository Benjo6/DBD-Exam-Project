using lib.DTO;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Storage;
using PrescriptionService.Models;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PharmacyController : ControllerBase
{
    private readonly IPharmacyStorage _storage;

    public PharmacyController(IPharmacyStorage storage)
    {
        _storage = storage;
    }

    [HttpGet("pharmacy")]
    public async Task<IEnumerable<PharmacyDto>> GetAllPharmacies([FromQuery] Page pageInfo)
        => await _storage.GetAll(pageInfo);
}
