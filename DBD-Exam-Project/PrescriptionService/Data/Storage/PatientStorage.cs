#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public class PatientStorage: IPatientStorage
{
    private readonly IAsyncRepository<Patient> _repo;
    private readonly IRedisCache _cache;
    private readonly IMapper _mapper;

    public PatientStorage(IAsyncRepository<Patient> repo, IRedisCache cache, IMapper mapper)
    {
        _repo = repo;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}";

        IEnumerable<Patient> cachedPatients = await _cache.BulkRetrive<Patient, int>(bulkKey);
        if (cachedPatients.Any())
            return cachedPatients.Select(_mapper.Map<Patient, PatientDto>);

        List<Patient> patients = await _repo
            .GetAll()
            .Skip(pageInfo.Number * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        if(patients.Any())
            await _cache.BulkStore<Patient, int>(patients, bulkKey);
        
        return patients.Select(_mapper.Map<Patient, PatientDto>);
    }
}
