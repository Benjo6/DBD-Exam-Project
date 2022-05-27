using lib.DTO;
using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Util;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PharmacyController : ControllerBase
{
    private readonly IAsyncRepository<Pharmacy> _pharmacyRepository;

    public PharmacyController(IAsyncRepository<Pharmacy> pharmacyRepository)
    {
        _pharmacyRepository = pharmacyRepository;
    }

    [HttpGet("pharmacy")]
    public IEnumerable<PharmacyDto> GetAllPharmacies()
        => _pharmacyRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToEnumerable();
}
