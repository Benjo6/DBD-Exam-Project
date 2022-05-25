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
using PrescriptionService.Data;
using PrescriptionService.Data.Repositories;

namespace PrescriptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IAsyncRepository<Patient> _patientRepository;
        private readonly IAsyncRepository<Pharmacy> _pharmacyRepository;

        public PrescriptionController(
            IPrescriptionRepository prescriptionRepository, 
            IAsyncRepository<Patient> patientRepository,
            IAsyncRepository<Pharmacy> pharmacyRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _pharmacyRepository = pharmacyRepository;
        }

        [HttpPost]
        public async Task<Prescription> Post([FromBody] Prescription prescription)
            => await _prescriptionRepository.Create(prescription);


        [HttpGet(Name = "GetPrescriptions")]
        public IEnumerable<PrescriptionDto> Get()
            => _prescriptionRepository
                .GetAllExpired()
                .Select(DtoMapper.ToDto);
        

        [HttpGet("{username}/{password}")]
        public IEnumerable<PrescriptionDto> GetForPatient(string username, string password)
            => _prescriptionRepository
                .GetAllForPatient(username)
                .Select(DtoMapper.ToDto);
        
        [HttpGet("patient")]
        public IEnumerable<PatientDto> GetAllPatients()
            => _patientRepository
                .GetAll()
                .Select(DtoMapper.ToDto)
                .ToEnumerable();

        [HttpGet("prescriptions")]
        public IEnumerable<PrescriptionDto> GetAllPrescriptions()
            => _prescriptionRepository
                .GetAll()
                .Select(DtoMapper.ToDto)
                .ToEnumerable();


        [HttpGet("pharmacy")]
        public IEnumerable<PharmacyDto> GetAllPharmacies()
            => _pharmacyRepository
                .GetAll()
                .Select(DtoMapper.ToDto)
                .ToEnumerable();

        [HttpPut("{id}")]
        public async Task<bool> Update(long id)
        {
            //var result = _prescriptionRepo.MarkPrescriptionWarningSent(id);
            var result = await _prescriptionRepository.Get(id);
            if (result is null)
                return false;

            result.ExpirationWarningSent = true;
            await _prescriptionRepository.Update(result);

            return true;
        }
    }
}
