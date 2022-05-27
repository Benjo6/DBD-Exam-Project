using lib.DTO;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IPatientStorage
{
    Task<IEnumerable<PatientDto>> GetAll(Page? pageInfo = null);
}