using lib.DTO;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Storage;
using PrescriptionService.Models;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PharmaciesController : ControllerBase
{
    private readonly IPharmacyStorage _storage;

    public PharmaciesController(IPharmacyStorage storage)
    {
        _storage = storage;
    }

    [HttpGet]
    public async Task<IEnumerable<PharmacyDto>> GetAllPharmacies([FromQuery] Page pageInfo)
        => await _storage.GetAll(pageInfo);
}
