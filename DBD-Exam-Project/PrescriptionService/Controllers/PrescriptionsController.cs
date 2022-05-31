#nullable enable
using lib.DTO;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Storage;
using PrescriptionService.Models;

namespace PrescriptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionStorage _storage;

        public PrescriptionsController(IPrescriptionStorage storage)
        {
            _storage = storage;
        }

        [HttpPost]
        public async Task<PrescriptionDto> Post([FromBody] PrescriptionDto prescription)
            => await _storage.Create(prescription);

        [HttpGet("{id}")]
        public async Task<PrescriptionDto> Get(long id)
            => await _storage.Get(id);

        [HttpGet("Expired")]
        public async Task<IEnumerable<PrescriptionDto>> GetExpiredPrescriptions([FromQuery] Page? pageInfo)
            => await _storage.GetAll(expired: true, pageInfo);


        [HttpGet("Patient/{cprNumber}")]
        public async Task<IEnumerable<PrescriptionDto>> GetForPatient(string cprNumber, [FromQuery] Page? pageInfo)
            => await _storage.GetAll(cprNumber, pageInfo);


        [HttpGet]
        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions([FromQuery] Page? pageInfo)
            => await _storage.GetAll(pageInfo: pageInfo);


        [HttpGet("Doctor/{doctorId}")]
        public async Task<IEnumerable<PrescriptionDto>> GetForDoctor(int doctorId, [FromQuery] Page? pageInfo)
            => await _storage.GetAll(doctorId, pageInfo);


        [HttpPut("{id}/MarkWarningSent")]
        public async Task<bool> MarkWarningSent(long id)
            => await _storage.MarkWarningAsSent(id);


        [HttpPut("FulfillPrescription")]
        public async Task<bool> FulfillPrescription([FromBody] FulfillPrescriptionRequestDto request)
            => await _storage.Fulfill(request.PrescriptionId, request.PharmaceutId);
    }
}
