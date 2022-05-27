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
    public class MedicineController : ControllerBase
    {
        private readonly IAsyncRepository<Medicine> _medicineRepository;
        public MedicineController(IAsyncRepository<Medicine> medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<MedicineDto>> GetAllMedicines()
            => await _medicineRepository
            .GetAll()
            .Select(DtoMapper.ToDto)
            .ToListAsync();

    }
}
