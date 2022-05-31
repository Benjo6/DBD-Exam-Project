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

    [HttpGet("booking/{longitude}/{latitude}/{distanceMeters}")]
    public async Task<IActionResult> GetConsultationBookings(double longitude, double latitude, int distanceMeters)
    {
        var result = await _consultationService.GetConsultationsOpenForBookingAsync(new GeoPointDto(longitude, latitude), distanceMeters);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("booking")]
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

    [HttpGet("{take}/{skip}")]
    public async Task<IActionResult> GetConsultations(int take, int skip)
    {
        var result = await _consultationService.GetConsultationsAsync(take, skip);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{consultationId}")]
    public async Task<IActionResult> DeleteConsultation(string consultationId)
    {
        return Ok(await _consultationService.DeleteConsultationAsync(consultationId));
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
