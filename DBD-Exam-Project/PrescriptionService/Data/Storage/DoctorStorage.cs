#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IDoctorStorage
{
    Task<IEnumerable<PersonDto>> GetAll(Page? pageInfo = null);
    Task<PersonDto> Get(int id);
}

public class DoctorStorage : BaseStorage<PersonDto, Doctor>, IDoctorStorage
{
    private readonly IAsyncRepository<Doctor> _repo;

    public DoctorStorage(IAsyncRepository<Doctor> repo, IRedisCache cache, IMapper mapper) : base(cache, mapper)
    {
        _repo = repo;
    }

    public Task<IEnumerable<PersonDto>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new Page();
        return GetAll(_repo.GetAll(), pageInfo);
    }

    public async Task<PersonDto> Get(int id)
    {
        Doctor? doctor = await Cache.Retrive<Doctor, int>(id);
        if (doctor == null)
        {
            doctor = await _repo.Get(id);
            if (doctor == null)
                return new();

            await Cache.Store<Doctor, int>(doctor);
        }
        return Mapper.Map<PersonDto>(doctor);
    }
}
