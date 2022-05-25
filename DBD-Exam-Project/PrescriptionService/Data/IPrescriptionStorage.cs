using lib.DTO;
using lib.Models;

namespace PrescriptionService.Data;

public interface IPrescriptionStorage
{
    Task<PrescriptionDto> Create(PrescriptionDto prescription, Doctor doctor)
    PrescriptionDto Update(PrescriptionDto prescription);
    IEnumerable<PrescriptionDto> GetAll(bool expired = false);
    IEnumerable<PrescriptionDto> GetAll(int userId, bool expired = false);
}