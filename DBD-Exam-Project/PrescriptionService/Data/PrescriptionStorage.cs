#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Models;

namespace PrescriptionService.Data;

public class PrescriptionStorage : IPrescriptionStorage
{
    private readonly IPrescriptionRepository _repo;
    private readonly IRedisCache _cache;
    private readonly IMapper _mapper;

    public PrescriptionStorage(IPrescriptionRepository repo, IRedisCache cache, IMapper mapper)
    {
        _repo = repo;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<PrescriptionDto> Create(PrescriptionDto prescription)
    {
        Prescription pre = _mapper.Map<Prescription>(prescription);
        pre = await _repo.Create(pre);
        await _cache.Store<Prescription, long>(pre);
        return _mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<PrescriptionDto> Update(PrescriptionDto prescription, int actorId)
    {
        Prescription pre = _mapper.Map<Prescription>(prescription);
        pre.LastAdministeredBy = actorId;
        pre = await _repo.Update(pre);
        await _cache.Store<Prescription, long>(pre);
        return _mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(bool expired = false, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = expired 
            ? $"p{pageInfo.Number}s{pageInfo.Size}" 
            : $"p{pageInfo.Number}s{pageInfo.Size}expired";


        IEnumerable<Prescription> cachedPrescriptions = await _cache.BulkRetrive<Prescription, long>(bulkKey);
        if (cachedPrescriptions.Any())
            return cachedPrescriptions.Select(_mapper.Map<Prescription, PrescriptionDto>);


        IAsyncEnumerable<Prescription> initialResult = expired 
            ? _repo.GetAllExpired() 
            : _repo.GetAll();

        List<Prescription> prescriptions = await initialResult
            .Skip(pageInfo.Number * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        await _cache.BulkStore<Prescription, long>(prescriptions, bulkKey);
        return prescriptions.Select(_mapper.Map<Prescription, PrescriptionDto>);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(string cprNumber, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}user{cprNumber}";
        IEnumerable<Prescription> cachedPrescriptions = await _cache.BulkRetrive<Prescription, long>(bulkKey);
        if (cachedPrescriptions.Any())
            return cachedPrescriptions.Select(_mapper.Map<Prescription, PrescriptionDto>);

        List<Prescription> prescriptions = await _repo.GetAllForPatient(cprNumber)
            .Skip(pageInfo.Number * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        await _cache.BulkStore<Prescription, long>(prescriptions, bulkKey);
        return prescriptions.Select(_mapper.Map<Prescription, PrescriptionDto>);
    }

    public async Task<PrescriptionDto> Get(long id)
    {
        Prescription? pre = await _cache.Retrive<Prescription, long>(id);
        if (pre == null)
        {
            pre = await _repo.Get(id);
            if(pre == null)
                return new();

            await _cache.Store<Prescription, long>(pre);
        }
        return _mapper.Map<PrescriptionDto>(pre);
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