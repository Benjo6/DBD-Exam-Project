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
}

public interface IPrescriptionService
{
    Task<IEnumerable<string>> GetMedicine();
}