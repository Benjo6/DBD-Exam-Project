using AutoMapper;
using lib.DTO;
using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Data.Storage;
using PrescriptionService.Util;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientStorage _storage;

    public PatientController(IPatientStorage storage)
    {
        _storage = storage;
    }

    [HttpGet]
    public async Task<IEnumerable<PatientDto>> GetAllPatients()
        => await _storage.GetAll();
}
