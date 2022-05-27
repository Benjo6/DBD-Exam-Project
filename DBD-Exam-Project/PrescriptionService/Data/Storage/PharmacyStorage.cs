using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public class PharmacyStorage : BaseStorage<PharmacyDto, Pharmacy>, IPharmacyStorage
{
    private readonly IAsyncRepository<Pharmacy> _repo;

    public PharmacyStorage(IAsyncRepository<Pharmacy> repo, IRedisCache cache, IMapper mapper)
        : base(cache, mapper)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<PharmacyDto>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}";

        return await GetAll(_repo.GetAll(), bulkKey, pageInfo);
    }
}