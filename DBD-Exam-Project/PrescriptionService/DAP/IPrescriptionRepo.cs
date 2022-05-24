using lib.Models;

namespace PrescriptionService.DAP;

public interface IPrescriptionRepo
{
    public IEnumerable<Prescription> GetPrescriptionsExpiringLatest(DateOnly expiringDate);
    public IEnumerable<Prescription> GetPrescriptionsForUser(string username, string password);
    public bool MarkPrescriptionWarningSent(long prescriptionId);
    public IEnumerable<Patient> GetAllPatients();
    public IEnumerable<Prescription> GetAllPrescriptions();
    public IEnumerable<Pharmacy> GetAllPharmacies();
}
