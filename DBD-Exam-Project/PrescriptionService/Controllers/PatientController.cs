using lib.DTO;
using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Util;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IAsyncRepository<Patient> _patientRepository;

    public PatientController(IAsyncRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    [HttpGet("patients")]
    public async Task<IEnumerable<PatientDto>> GetAllPatients()
        => await _patientRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToListAsync();
}
