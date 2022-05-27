using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public class PharmacyStorage : IPharmacyStorage
{
    private readonly IAsyncRepository<Pharmacy> _repo;
    private readonly IRedisCache _cache;
    private readonly IMapper _mapper;

    public PharmacyStorage(IAsyncRepository<Pharmacy> repo, IRedisCache cache, IMapper mapper)
    {
        _repo = repo;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PharmacyDto>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}";

        IEnumerable<Pharmacy> cachedPharmacies = await _cache.BulkRetrive<Pharmacy, int>(bulkKey);
        if (cachedPharmacies.Any())
            return cachedPharmacies.Select(_mapper.Map<Pharmacy, PharmacyDto>);

        List<Pharmacy> pharmacies = await _repo
            .GetAll()
            .Skip(pageInfo.Number * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        if (pharmacies.Any())
            await _cache.BulkStore<Pharmacy, int>(pharmacies, bulkKey);

        return pharmacies.Select(_mapper.Map<Pharmacy, PharmacyDto>);
    }
}