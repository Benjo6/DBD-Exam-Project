using ConsultationService.Services;
using lib.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ConsultationService.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsultationController : ControllerBase
{

    private readonly ILogger<ConsultationController> _logger;
    private readonly IConsultationService _consultationService;

    public ConsultationController(ILogger<ConsultationController> logger, IConsultationService consultationService)
    {
        _logger = logger;
        _consultationService = consultationService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ConsultationDto GetConsultation(string consultationId)
    {
        return _consultationService.GetConsultation(consultationId);
    }

    public IEnumerable<ConsultationDto> GetConsultationsForPatient(string patientId)
    {
        return _consultationService.GetConsultationsForPatient(patientId);
    }

    public IEnumerable<ConsultationDto> GetConsultationsForDoctor(string doctorId)
    {
        return _consultationService.GetConsultationsForDoctor(doctorId);
    }
}
