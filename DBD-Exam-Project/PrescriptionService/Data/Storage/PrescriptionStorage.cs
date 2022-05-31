#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IPrescriptionStorage
{
    Task<PrescriptionDto> Create(PrescriptionCreationDto prescription);
    Task<IEnumerable<PrescriptionDto>> GetAll(bool expired = false, Page? pageInfo = null);
    Task<IEnumerable<PrescriptionDto>> GetAll(string cprNumber, Page? pageInfo = null);
    Task<IEnumerable<PrescriptionDto>> GetAll(int doctorId, Page? pageInfo = null);
    Task<PrescriptionDto> Get(long id);
    Task<bool> MarkWarningAsSent(long id);
    Task<bool> Fulfill(long id, int pharmaceutId);
}

public class PrescriptionStorage : BaseStorage<PrescriptionDto, Prescription, long>, IPrescriptionStorage
{
    private readonly IPrescriptionRepository _repo;
    private readonly IPatientStorage _patientStorage;

    public PrescriptionStorage(IPrescriptionRepository repo, IPatientStorage patientStorage, IRedisCache cache, IMapper mapper)
        : base(cache, mapper)
    {
        _repo = repo;
        _patientStorage = patientStorage;
    }

    public async Task<PrescriptionDto> Create(PrescriptionCreationDto prescription)
    {
        Prescription pre = Mapper.Map<PrescriptionCreationDto, Prescription>(prescription);

        PersonDto p = await _patientStorage.Get(pre.PrescribedToCpr);
        pre.PrescribedTo = p.Id;
        pre.Creation = DateTime.Now;

        pre = await _repo.Create(pre);
        await Cache.Store<Prescription, long>(pre);
        return Mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(bool expired = false, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = expired 
            ? $"p{pageInfo.Number}s{pageInfo.Size}" 
            : $"p{pageInfo.Number}s{pageInfo.Size}expired";

        return await GetAll(() => expired ? _repo.GetAllExpired() : _repo.GetAll(), bulkKey, pageInfo);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(string cprNumber, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}user{cprNumber}";
        return await GetAll(() => _repo.GetAllForPatient(cprNumber), bulkKey, pageInfo);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAll(int doctorId, Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}doctor{doctorId}";
        return await GetAll(() => _repo.GetAllForDoctor(doctorId), bulkKey, pageInfo);
    }

    public async Task<PrescriptionDto> Get(long id)
    {
        Prescription? pre = await Cache.Retrive<Prescription, long>(id);
        if (pre == null)
        {
            pre = await _repo.GetDetailed(id);
            if(pre == null)
                return new();

            await Cache.Store<Prescription, long>(pre);
        }
        return Mapper.Map<PrescriptionDto>(pre);
    }

    public async Task<bool> MarkWarningAsSent(long id)
    {
        Prescription? result = await Cache.Retrive<Prescription, long>(id) ?? await _repo.Get(id);
        if (result is null)
            return false;

        result.ExpirationWarningSent = true;
        result = await _repo.Update(result);
        await Cache.Store<Prescription, long>(result);

        return true;
    }

    public async Task<bool> Fulfill(long id, int pharmaceutId)
    {
        Prescription? result = await Cache.Retrive<Prescription, long>(id) ?? await _repo.Get(id);
        if (result is null)
            return false;

        result.LastAdministeredBy = pharmaceutId;
        result = await _repo.Update(result);
        await Cache.Store<Prescription, long>(result);

        return true;
    }
}