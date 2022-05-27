using lib.DTO;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IPharmacyStorage
{
    Task<IEnumerable<PharmacyDto>> GetAll(Page? page = null);
}