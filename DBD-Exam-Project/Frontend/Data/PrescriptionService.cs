using System.Text.Json;
using lib.DTO;

namespace Frontend.Data;

public class PeopleService : IPeopleService
{
    private readonly HttpClient _client;

    public PeopleService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("PrescriptionClient");
    }

    public async Task<IEnumerable<PersonDto>> GetPeopleAsync()
        => await _client.GetFromJsonAsync<IEnumerable<PersonDto>>(GetPeopleBaseUrl(null))
            ?? Enumerable.Empty<PersonDto>();

    public async Task<IEnumerable<PersonDto>> GetPeopleAsync(PersonType type)
        => await _client.GetFromJsonAsync<IEnumerable<PersonDto>>(GetPeopleBaseUrl(type)) 
            ?? Enumerable.Empty<PersonDto>();

    public async Task<PersonDto> GetPersonDetails(int id, PersonType type)
        => await _client.GetFromJsonAsync<PersonDto>($"{GetPeopleBaseUrl(type)}/{id}")
            ?? new();

    public async Task<PersonDto> GetPatientDetails(string cprNumber)
        => await _client.GetFromJsonAsync<PersonDto>($"{GetPeopleBaseUrl(PersonType.Patient)}/cpr/{cprNumber}") 
            ?? new();

    private string GetPeopleBaseUrl(PersonType? type)
        => type switch
        {
            PersonType.Patient => "Persons/patients",
            PersonType.Doctor => "Persons/doctors",
            PersonType.Pharmaceut => "Persons/pharmaceuts",
            _ => "Persons"
        };
}

public interface IPeopleService
{
    Task<IEnumerable<PersonDto>> GetPeopleAsync();
    Task<IEnumerable<PersonDto>> GetPeopleAsync(PersonType type);
    Task<PersonDto> GetPersonDetails(int id, PersonType type);
    Task<PersonDto> GetPatientDetails(string cprNumber);
}
