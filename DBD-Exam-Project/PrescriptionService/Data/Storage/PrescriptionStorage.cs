#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public class PrescriptionStorage : BaseStorage<PrescriptionDto, Prescription, long>, IPrescriptionStorage
{
    private readonly IPrescriptionRepository _repo;

    public PrescriptionStorage(IPrescriptionRepository repo, IRedisCache cache, IMapper mapper)
        : base(cache, mapper)
    {
        _repo = repo;
    }

    public async Task<PrescriptionDto> Create(PrescriptionDto prescription)
    {
        Prescription pre = Mapper.Map<Prescription>(prescription);
        pre = await _repo.Create(pre);
        await Cache.Store<Prescription, long>(pre);
        return Mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<PrescriptionDto> Update(PrescriptionDto prescription, int actorId)
    {
        Prescription pre = Mapper.Map<Prescription>(prescription);
        pre.LastAdministeredBy = actorId;
        pre = await _repo.Update(pre);
        await Cache.Store<Prescription, long>(pre);
        return Mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(bool expired = false, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = expired 
            ? $"p{pageInfo.Number}s{pageInfo.Size}" 
            : $"p{pageInfo.Number}s{pageInfo.Size}expired";

        return await GetAll(expired ? _repo.GetAllExpired() : _repo.GetAll(), bulkKey, pageInfo);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(string cprNumber, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}user{cprNumber}";
        return await GetAll(_repo.GetAllForPatient(cprNumber), bulkKey, pageInfo);
    }

    public async Task<PrescriptionDto> Get(long id)
    {
        Prescription? pre = await Cache.Retrive<Prescription, long>(id);
        if (pre == null)
        {
            pre = await _repo.Get(id);
            if(pre == null)
                return new();

            await Cache.Store<Prescription, long>(pre);
        }
        return Mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<bool> MarkWarningAsSent(long id)
    {
        var result = await _repo.Get(id);
        if (result is null)
            return false;

        result.ExpirationWarningSent = true;
        await _repo.Update(result);

        return true;
    }
}