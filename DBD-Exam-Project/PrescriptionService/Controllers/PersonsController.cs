#nullable enable
using lib.DTO;
using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Data.Storage;
using PrescriptionService.Models;

namespace PrescriptionService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPatientStorage _patientStorage;
    private readonly IDoctorStorage _doctorStorage;
    private readonly IPharmaceutStorage _pharmaceutStorage;

    public PersonsController(IPatientStorage patientStorage, IDoctorStorage doctorStorage, IPharmaceutStorage pharmaceutStorage)
    {
        _patientStorage = patientStorage;
        _doctorStorage = doctorStorage;
        _pharmaceutStorage = pharmaceutStorage;
    }

    [HttpGet]
    public async Task<IEnumerable<PersonDto>> GetAll([FromQuery] Page? pageInfo)
    {
        Page individualPages = new()
        {
            Number = pageInfo?.Number ?? 1,
            Size = (pageInfo?.Size ?? 90) / 3
        };

        return new List<PersonDto>()
            .Concat(await _pharmaceutStorage.GetAll(individualPages))
            .Concat(await _doctorStorage.GetAll(individualPages))
            .Concat(await _patientStorage.GetAll(individualPages));
    }

    [HttpGet("patients")]
    public async Task<IEnumerable<PersonDto>> GetAllPatients([FromQuery] Page? pageInfo)
        => await _patientStorage.GetAll(pageInfo);

    [HttpGet("patients/{id}")]
    public async Task<PersonDto> GetPatient(int id)
        => await _patientStorage.Get(id);

    [HttpGet("patients/cpr/{cprNumber}")]
    public async Task<PersonDto> GetPatient(string cprNumber)
        => await _patientStorage.Get(cprNumber);

    [HttpGet("doctors")]
    public async Task<IEnumerable<PersonDto>> GetAllDoctors([FromQuery] Page? pageInfo)
        => await _doctorStorage.GetAll(pageInfo);

    [HttpGet("doctors/{id}")]
    public async Task<PersonDto> GetDoctor(int id)
        => await _doctorStorage.Get(id);

    [HttpGet("pharmaceuts")]
    public async Task<IEnumerable<PersonDto>> GetAllPharmaceuts([FromQuery] Page? pageInfo)
        => await _pharmaceutStorage.GetAll(pageInfo);

    [HttpGet("pharmaceuts/{id}")]
    public async Task<PersonDto> GetPharmaceut(int id)
        => await _pharmaceutStorage.Get(id);
}
