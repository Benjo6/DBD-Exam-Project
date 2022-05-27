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
public class PatientController : ControllerBase
{
    private readonly IAsyncRepository<Patient> _patientRepository;
    private readonly IMapper _mapper;

    public PatientController(IAsyncRepository<Patient> patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    [HttpGet("patients")]
    public async Task<IEnumerable<PatientDto>> GetAllPatients()
        => await _patientRepository
            .GetAll()
            .Select(_mapper.Map<Patient, PatientDto>)
            .ToListAsync();
}
