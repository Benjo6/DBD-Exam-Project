#nullable enable
using AutoMapper;
using lib.DTO;
using lib.Models;
using PrescriptionService.Data.Repositories;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public class PatientStorage: BaseStorage<PatientDto, Patient>, IPatientStorage
{
    private readonly IAsyncRepository<Patient> _repo;

    public PatientStorage(IAsyncRepository<Patient> repo, IRedisCache cache, IMapper mapper) 
        : base(cache, mapper)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<PatientDto>> GetAll(Page? pageInfo = null)
    {
        pageInfo ??= new();

        string bulkKey = $"p{pageInfo.Number}s{pageInfo.Size}";

        return await GetAll(_repo.GetAll(), bulkKey, pageInfo);
    }
}
