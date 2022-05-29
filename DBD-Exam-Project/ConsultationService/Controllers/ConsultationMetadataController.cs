using ConsultationService.Services;
using lib.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ConsultationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultationMetadataController : ControllerBase
{

    private readonly ILogger<ConsultationController> _logger;
    private readonly IConsultationMetadataService _consultationMetadataService;

    public ConsultationMetadataController(ILogger<ConsultationController> logger, IConsultationMetadataService consultationMetadataService)
    {
        _logger = logger;
        _consultationMetadataService = consultationMetadataService;
    }   

    [HttpPost]
    public async Task<IActionResult> CreateConsultationMetadata(ConsultationMetadataDto consultationMetadataDto)
    {
        return Ok(await _consultationMetadataService.AddMetadataAsync(consultationMetadataDto));
    }

    [HttpGet]
    public async Task<IActionResult> GetLatestConsultationMetadata()
    {
        var result = await _consultationMetadataService.GetLatestMetadataAsync();

        if (result == null)
          return NotFound();

        return Ok(result);
    }
}
