#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IPatientStorage
{
    Task<IEnumerable<PersonDto>> GetAll(Page? pageInfo = null);
    Task<PersonDto> Get(int id);
    Task<PersonDto> Get(string cpr);
}

public class PatientStorage: BaseStorage<PersonDto, Patient>, IPatientStorage
{
    private readonly IPatientRepository _repo;

    public PatientStorage(IPatientRepository repo, IRedisCache cache, IMapper mapper) 
        : base(cache, mapper)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<PersonDto>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}";

        return await GetAll(() => _repo.GetAll(), bulkKey, pageInfo);
    }

    public async Task<PersonDto> Get(int id)
    {
        Patient? patient = await Cache.Retrive<Patient, int>(id);
        if (patient == null)
        {
            patient = await _repo.Get(id);
            if (patient == null)
                return new();

            await Cache.Store<Patient, int>(patient);
        }
        return Mapper.Map<PersonDto>(patient);
    }

    public async Task<PersonDto> Get(string cpr)
    {
        string key = $"cpr{cpr}";
        Patient? patient = await Cache.Retrive<Patient, string>(key);
        if (patient == null)
        {
            patient = await _repo.GetByCpr(cpr);
            if (patient == null)
                return new();

            await Cache.Store(patient, key);
        }
        return Mapper.Map<PersonDto>(patient);
    }
}
