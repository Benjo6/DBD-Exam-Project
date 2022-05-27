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
    public class PharmaceutController : ControllerBase
    {
        private readonly IAsyncRepository<Pharmaceut> _pharmaceutRespository;

        public PharmaceutController(IAsyncRepository<Pharmaceut> pharmaceutRespository)
        {
            _pharmaceutRespository = pharmaceutRespository;
        }

        [HttpGet]
        public async Task<IEnumerable<PharamceutDto>> GetAllMedicines()
                  => await _pharmaceutRespository
                  .GetAll()
                  .Select(DtoMapper.ToDto)
                  .ToListAsync();
    }
}
