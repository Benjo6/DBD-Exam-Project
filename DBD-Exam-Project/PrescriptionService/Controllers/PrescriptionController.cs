using lib.DTO;
using lib.Models;
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
        private readonly IPrescriptionRepo _prescriptionRepo;

        public PrescriptionController(IPrescriptionRepo prescriptionRepo)
        {
            _prescriptionRepo = prescriptionRepo;

        }


        [HttpGet(Name = "GetPrescriptions")]
        public IEnumerable<PrescriptionDto> Get()
        {
            var result = _prescriptionRepo.GetPrescriptionsExpiringLatest(DateOnly.FromDateTime(DateTime.Now.AddDays(7))).Select(x => PrescriptionMapper.ToDto(x));
            return result;
        }

        [HttpGet("{username}/{password}")]
        public IEnumerable<PrescriptionDto> GetForPatient(string username, string password)
        {
            var result = _prescriptionRepo.GetPrescriptionsForUser(username, password).Select(x => PrescriptionMapper.ToDto(x));
            return result;
        }
        [HttpGet("patient")]
        public IEnumerable<Patient> GetAllPatients()
        {
            var result = _prescriptionRepo.GetAllPatients();
            return result;
        }

        [HttpGet("prescriptions")]
        public IEnumerable<PrescriptionDto> GetAllPrescriptions()
        {
            var result = _prescriptionRepo.GetAllPrescriptions().Select(x => PrescriptionMapper.ToDto(x));
            return result;
        }

        [HttpGet("pharmacy")]
        public IEnumerable<Pharmacy> GetAllPharmacies()
        {
            var result = _prescriptionRepo.GetAllPharmacies();
            return result;
        }

        [HttpPut("{id}")]
        public bool Update(long id)
        {
            var result = _prescriptionRepo.MarkPrescriptionWarningSent(id);
            return result;
        }
    }
}
