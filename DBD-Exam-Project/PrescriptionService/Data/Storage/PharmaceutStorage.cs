#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IPharmaceutStorage
{
    Task<IEnumerable<PersonDto>> GetAll(Page? pageInfo = null);
    Task<PersonDto> Get(int id);
}

public class PharmaceutStorage : BaseStorage<PersonDto, Pharmaceut>, IPharmaceutStorage
{
    private readonly IAsyncRepository<Pharmaceut> _repo;

    public PharmaceutStorage(IAsyncRepository<Pharmaceut> repo, IRedisCache cache, IMapper mapper) : base(cache, mapper)
    {
        _repo = repo;
    }

    public Task<IEnumerable<PersonDto>> GetAll(Page? pageInfo = null)
        => GetAll(_repo.GetAll(), pageInfo ?? new());

    public async Task<PersonDto> Get(int id)
    {
        Pharmaceut? pharmaceut = await Cache.Retrive<Pharmaceut, int>(id);
        if (pharmaceut == null)
        {
            pharmaceut = await _repo.Get(id);
            if (pharmaceut == null)
                return new();

            await Cache.Store<Pharmaceut, int>(pharmaceut);
        }
        return Mapper.Map<PersonDto>(pharmaceut);
    }

    
}
