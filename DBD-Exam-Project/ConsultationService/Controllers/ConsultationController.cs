using ConsultationService.Services;
using lib.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ConsultationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultationController : ControllerBase
{

    private readonly ILogger<ConsultationController> _logger;
    private readonly IConsultationService _consultationService;

    public ConsultationController(ILogger<ConsultationController> logger, IConsultationService consultationService)
    {
        _logger = logger;
        _consultationService = consultationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateConsultation(ConsultationCreationDto consultationDto)
    {
        return Ok(await _consultationService.CreateConsultationAsync(consultationDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateConsultation(ConsultationDto consultationDto)
    {
        var result = await _consultationService.UpdateConsultationAsync(consultationDto);

        if (result == null)
          return NotFound();

        return Ok(result);
    }

    [HttpPut("book")]
    public async Task<IActionResult> BookConsultation(ConsultationBookingRequestDto consultationDto)
    {
        var result = await _consultationService.BookConsultationAsync(consultationDto);

        if (result == null)
          return NotFound();

        return Ok(result);
    }

    [HttpGet("{consultationId}")]
    public async Task<IActionResult> GetConsultation(string consultationId)
    {
        var result = await _consultationService.GetConsultationAsync(consultationId);
        if (result == null)
          return NotFound();

        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetConsultationsForPatient(string patientId)
    {
        var result = await _consultationService.GetConsultationsForPatientAsync(patientId);

        if (result == null)
          return NotFound();

        return Ok(result);
    }
    
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetConsultationsForDoctor(string doctorId)
    {
        var result = await _consultationService.GetConsultationsForDoctorAsync(doctorId);

        if (result == null)
          return NotFound();

        return Ok(result);
    }
}
