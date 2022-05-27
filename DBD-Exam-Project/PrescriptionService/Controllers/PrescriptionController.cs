using lib.DTO;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Storage;
using PrescriptionService.Models;

namespace PrescriptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionStorage _storage;

        public PrescriptionController(IPrescriptionStorage storage)
        {
            _storage = storage;
        }

        [HttpPost]
        public async Task<PrescriptionDto> Post([FromBody] PrescriptionDto prescription)
            => await _storage.Create(prescription);


        [HttpGet("expired")]
        public async Task<IEnumerable<PrescriptionDto>> Get([FromQuery] Page pageInfo)
            => await _storage.GetAll(expired: true, pageInfo);


        [HttpGet("cprNumber")]
        public async Task<IEnumerable<PrescriptionDto>> GetForPatient(string cprNumber, [FromQuery] Page pageInfo)
            => await _storage.GetAll(cprNumber, pageInfo);


        [HttpGet]
        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions([FromQuery] Page pageInfo)
            => await _storage.GetAll(pageInfo: pageInfo);


        [HttpPut("{id}")]
        public async Task<bool> MarkWarningSent(long id)
            => await _storage.MarkWarningAsSent(id);
    }
}
