using AutoMapper;
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
    private readonly IMapper _mapper;

    public PharmacyController(IAsyncRepository<Pharmacy> pharmacyRepository, IMapper mapper)
    {
        _pharmacyRepository = pharmacyRepository;
        _mapper = mapper;
    }

    [HttpGet("pharmacy")]
    public IEnumerable<PharmacyDto> GetAllPharmacies()
        => _pharmacyRepository
            .GetAll()
            .Select(_mapper.Map<Pharmacy, PharmacyDto>)
            .ToEnumerable();
}
