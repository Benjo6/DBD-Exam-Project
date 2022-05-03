using lib.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrescriptionService.DAP;
using PrescriptionService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly ILogger<PrescriptionController> _logger;
        private readonly IPrescriptionRepo _prescriptionRepo;

        public PrescriptionController(ILogger<PrescriptionController> logger, IPrescriptionRepo prescriptionRepo)
        {
            _logger = logger;
            _prescriptionRepo = prescriptionRepo;

        }


        [HttpGet(Name ="GetPrescriptions")]
        public IEnumerable<PrescriptionDto> Get()
        {
            var result = _prescriptionRepo.GetPrescriptionsExpiringLatest(DateTime.Now.AddDays(7)).Select(x => PrescriptionMapper.ToDto(x));
        }
    }
}
