using lib.DTO;

namespace Frontend.Data;

public class PrescriptionService: IPrescriptionService
{
    private readonly HttpClient _client;

    public PrescriptionService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("PrescriptionClient");
    }

    public async Task<IEnumerable<string>> GetMedicine()
        => await _client.GetFromJsonAsync<IEnumerable<string>>("Medicines")
            ?? Enumerable.Empty<string>();

    public Task<bool> FulfillPrescription(long id)
    {
        return Task.FromResult(true);
    }

    public Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions(int pageCount = 0, int pageSize = 0)
    {
        return Task.FromResult(Enumerable.Empty<PrescriptionDto>());
    }

    public Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForDoctor(int doctorId, int pageCount = 0, int pageSize = 0)
    {
        return Task.FromResult(Enumerable.Empty<PrescriptionDto>());
    }

    public Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForPatient(string cprNumber, int pageCount = 0, int pageSize = 0)
    {
        return Task.FromResult(Enumerable.Empty<PrescriptionDto>());
    }

    public Task<PrescriptionDto> CreatePrescription(PrescriptionDto newPrescription)
    {
        return Task.FromResult(new PrescriptionDto());
    }
}

public interface IPrescriptionService
{
    Task<IEnumerable<string>> GetMedicine();
    Task<bool> FulfillPrescription(long id);
    Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions(int pageCount = 0, int pageSize = 0);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForDoctor(int doctorId, int pageCount = 0, int pageSize = 0);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForPatient(string cprNumber, int pageCount = 0, int pageSize = 0);
    Task<PrescriptionDto> CreatePrescription(PrescriptionDto newPrescription);
    
}