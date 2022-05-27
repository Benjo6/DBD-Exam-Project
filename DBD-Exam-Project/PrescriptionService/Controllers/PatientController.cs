using lib.DTO;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Storage;

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
