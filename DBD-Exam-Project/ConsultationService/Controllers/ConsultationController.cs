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
    public async Task<ConsultationDto> CreateConsultation(ConsultationCreationDto consultationDto)
    {
        return await _consultationService.CreateConsultationAsync(consultationDto);
    }

    [HttpPut]
    public async Task<ConsultationDto> UpdateConsultation(ConsultationDto consultationDto)
    {
        return await _consultationService.UpdateConsultationAsync(consultationDto);
    }

    [HttpPut]
    public async Task<ConsultationDto> BookConsultation(ConsultationBookingRequestDto consultationDto)
    {
        return await _consultationService.BookConsultationAsync(consultationDto);
    }

    [HttpGet("{consultationId}")]
    public ConsultationDto GetConsultation(string id)
    {
        return _consultationService.GetConsultation(id);
    }

    [HttpGet("patient/{patientId}")]
    public IEnumerable<ConsultationDto> GetConsultationsForPatient(string patientId)
    {
        return _consultationService.GetConsultationsForPatient(patientId);
    }
    
    [HttpGet("doctor/{doctorId}")]
    public IEnumerable<ConsultationDto> GetConsultationsForDoctor(string doctorId)
    {
        return _consultationService.GetConsultationsForDoctor(doctorId);
    }
}
