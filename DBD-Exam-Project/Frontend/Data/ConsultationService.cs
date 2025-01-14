﻿using lib.DTO;
using System.Globalization;

namespace Frontend.Data;

public class ConsultationService : IConsultationService
{
    private HttpClient _client;

    public ConsultationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ConsultationClient");
    }

    public async Task BookAvailableConsultationAsync(ConsultationBookingRequestDto dto)
    {
        await _client.PutAsJsonAsync("Consultation/booking", dto);
    }


    public async Task<List<ConsultationDto>> GetAvailableConsultationsAsync(double longitude, double latitude, int distance)
        => await _client.GetFromJsonAsync<List<ConsultationDto>>($"Consultation/booking/{longitude.ToString(CultureInfo.InvariantCulture)}/{latitude.ToString(CultureInfo.InvariantCulture)}/{distance}")
            ?? new List<ConsultationDto>();

    public async Task<List<ConsultationDto>> GetPersonalizedConsultationsPatient(String id)
        => await _client.GetFromJsonAsync<List<ConsultationDto>>($"Consultation/patient/{id}")
            ?? new List<ConsultationDto>();

    public async Task<List<ConsultationDto>> GetPersonalizedConsultationsDoctor(String id)
    => await _client.GetFromJsonAsync<List<ConsultationDto>>($"Consultation/doctor/{id}")
        ?? new List<ConsultationDto>();
}

public interface IConsultationService
{
    Task<List<ConsultationDto>> GetAvailableConsultationsAsync(double longitude, double latitude, int distance);
    Task BookAvailableConsultationAsync(ConsultationBookingRequestDto dto);
    Task<List<ConsultationDto>> GetPersonalizedConsultationsPatient(String id);
    Task<List<ConsultationDto>> GetPersonalizedConsultationsDoctor(String id);
}