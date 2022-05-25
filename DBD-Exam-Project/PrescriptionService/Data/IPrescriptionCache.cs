using lib.Models;

namespace PrescriptionService.Data;

public interface IPrescriptionCache
{
    Task StorePrescription(Prescription prescription);
    Task<Prescription> RetrivePrescription(long id);
}
