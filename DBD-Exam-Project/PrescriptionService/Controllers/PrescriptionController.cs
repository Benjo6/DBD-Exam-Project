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
        private readonly IAsyncRepository<Medicine> _medicineRepository;
        private readonly IAsyncRepository<Doctor> _doctorRepository;

        public PrescriptionController(
            IPrescriptionRepository prescriptionRepository, 
            IAsyncRepository<Patient> patientRepository,
            IAsyncRepository<Pharmacy> pharmacyRepository
            ,IAsyncRepository<Doctor> doctorRepository,
            IAsyncRepository<Medicine> medicineRepository)
        {
            _doctorRepository = doctorRepository;
            _medicineRepository = medicineRepository;
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _pharmacyRepository = pharmacyRepository;
        }

        [HttpPost]
        public async Task<Prescription> Post([FromBody] Prescription prescription)
            => await _prescriptionRepository.Create(prescription);


        [HttpGet(Name = "GetPrescriptions")]
        public async Task<IEnumerable<PrescriptionDto>> Get([FromQuery] int count = 100)
            => await _prescriptionRepository
                .GetAllExpired()
                .Take(count)
                .Select(DtoMapper.ToDto)
                .ToListAsync();
        

        [HttpGet("{username}/{password}")]
        public async Task<IEnumerable<PrescriptionDto>> GetForPatient(string username, string password)
            => await _prescriptionRepository
                .GetAllForPatient(username)
                .Select(DtoMapper.ToDto)
                .ToListAsync();


        [HttpGet("medicine")]
        public async Task<IEnumerable<MedicineDto>> GetAllMedicines()
            => await _medicineRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToListAsync();


        [HttpGet("patient")]
        public async Task<IEnumerable<PatientDto>> GetAllPatients()
            => await _patientRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToListAsync();


        [HttpGet("doctor")]
        public async Task<IEnumerable<DoctorDto>> GetDoctors()
            => await _doctorRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToListAsync();

        [HttpGet("prescriptions")]
        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions()
            => await _prescriptionRepository
                .GetAll()
                .Select(DtoMapper.ToDto)
                .ToListAsync();


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
