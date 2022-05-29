#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IMedicineStorage
{
    Task<IEnumerable<string>> GetAll(Page? pageInfo = null);
    Task<MedicineDto> Get(string name);
}

public class MedicineStorage : BaseStorage<string, Medicine>, IMedicineStorage
{
    private readonly IMedicineRepository _repo;

    public MedicineStorage(IMedicineRepository repo, IRedisCache cache, IMapper mapper) 
        : base(cache, mapper)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<string>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new();
        
        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}";

        return await GetAll(() => _repo.GetAll(), bulkKey, pageInfo);
    }

    public async Task<MedicineDto> Get(string name)
    {
        Medicine? pre = await Cache.Retrive<Medicine, string>(name);
        if (pre == null)
        {
            pre = await _repo.Get(name);
            if (pre == null)
                return new();

            await Cache.Store(pre, name);
        }
        return Mapper.Map<MedicineDto>(pre);
    }
}
