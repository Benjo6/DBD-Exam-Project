using lib.DTO;

namespace Frontend.Data;

public class ConsultationService : IConsultationService
{
    private HttpClient _client;

    public ConsultationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ConsultationClient");
    }

    public async Task<List<ConsultationDto>> GetAvailableConsultationsAsync(double longitude, double latitude, int distance)
        => await _client.GetFromJsonAsync<List<ConsultationDto>>($"Consultation/booking/{longitude}/{latitude}/{distance}")
            ?? new List<ConsultationDto>();
}

public interface IConsultationService
{
    Task<List<ConsultationDto>> GetAvailableConsultationsAsync(double longitude, double latitude, int distance);
}