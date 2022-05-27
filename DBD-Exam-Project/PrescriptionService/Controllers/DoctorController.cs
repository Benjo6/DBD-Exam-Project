using lib.DTO;
using lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Util;

namespace PrescriptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IAsyncRepository<Doctor> _doctorRepository;

        public DoctorController(IAsyncRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<DoctorDto>> GetDoctors()
            => await _doctorRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToListAsync();
    }
}
