using Frontend.Authentication;
using lib.DTO;

namespace Frontend.Data;

public class PrescriptionService: BasePrescriptionService, IPrescriptionService
{
    private readonly UserProvider _userProvider;
    private readonly HttpClient _client;

    public PrescriptionService(IHttpClientFactory factory, UserProvider userProvider)
    {
        _userProvider = userProvider;
        _client = factory.CreateClient("PrescriptionClient");
    }

    public async Task<IEnumerable<string>> GetMedicine()
        => await _client.GetFromJsonAsync<IEnumerable<string>>("Medicines")
            ?? Enumerable.Empty<string>();

    public async Task<IEnumerable<PharmacyDto>> GetPharmacies(int pageCount = 0, int pageSize = 0)
        => await _client.GetFromJsonAsync<IEnumerable<PharmacyDto>>("Pharmacies" + GetPageParameter(pageCount, pageSize))
            ?? Enumerable.Empty<PharmacyDto>();

    public async Task<bool> FulfillPrescription(long id)
    {
        if (_userProvider.CurrentUser?.Type != PersonType.Pharmaceut)
            throw new Exception("User must be pharmaceut to use this method!");

        FulfillPrescriptionRequestDto dto = new() { PrescriptionId = id, PharmaceutId = _userProvider.CurrentUser.Id };
        HttpResponseMessage resp = await _client.PutAsJsonAsync("Prescriptions/FulfillPrescription", dto);
        resp.EnsureSuccessStatusCode();

        return await resp.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions(int pageCount = 0, int pageSize = 0)
        => await _client.GetFromJsonAsync<IEnumerable<PrescriptionDto>>("Prescriptions" + GetPageParameter(pageCount, pageSize))
            ?? Enumerable.Empty<PrescriptionDto>();

    public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForDoctor(int doctorId, int pageCount = 0, int pageSize = 0)
        => await _client.GetFromJsonAsync<IEnumerable<PrescriptionDto>>($"Prescriptions/Doctor/{doctorId}" + GetPageParameter(pageCount, pageSize))
            ?? Enumerable.Empty<PrescriptionDto>();

    public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForPatient(string cprNumber, int pageCount = 0, int pageSize = 0)
        => await _client.GetFromJsonAsync<IEnumerable<PrescriptionDto>>($"Prescriptions/Patient/{cprNumber}" + GetPageParameter(pageCount, pageSize))
            ?? Enumerable.Empty<PrescriptionDto>();

    public async Task<PrescriptionDto> CreatePrescription(PrescriptionDto newPrescription)
    {
        PrescriptionCreationDto prescriptionCreationDto = new()
        {
            MedicineName = newPrescription.MedicineName,
            DoctorId = newPrescription.Doctor.Id,
            PatientCprNumber = newPrescription.Patient.CphNumber,
            Expiration = newPrescription.Expiration.Value

        };
        HttpResponseMessage resp = await _client.PostAsJsonAsync("Prescriptions", prescriptionCreationDto);
        resp.EnsureSuccessStatusCode();

        return await resp.Content.ReadFromJsonAsync<PrescriptionDto>() 
            ?? new();
    }

    public async Task<PrescriptionDto> GetPrescription(long id)
        => await _client.GetFromJsonAsync<PrescriptionDto>($"Prescriptions/{id}")
           ?? new();
}

public interface IPrescriptionService
{
    Task<IEnumerable<string>> GetMedicine();
    Task<IEnumerable<PharmacyDto>> GetPharmacies(int pageCount = 0, int pageSize = 0);
    Task<bool> FulfillPrescription(long id);
    Task<IEnumerable<PrescriptionDto>> GetAllPrescriptions(int pageCount = 0, int pageSize = 0);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForDoctor(int doctorId, int pageCount = 0, int pageSize = 0);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsForPatient(string cprNumber, int pageCount = 0, int pageSize = 0);
    Task<PrescriptionDto> CreatePrescription(PrescriptionDto newPrescription);
    Task<PrescriptionDto> GetPrescription(long id);
}