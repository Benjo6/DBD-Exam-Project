#nullable enable
using lib.DTO;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public interface IPrescriptionStorage
{
    Task<PrescriptionDto> Create(PrescriptionDto prescription);
    Task<IEnumerable<PrescriptionDto>> GetAll(bool expired = false, Page? pageInfo = null);
    Task<IEnumerable<PrescriptionDto>> GetAll(string cprNumber, Page? pageInfo = null);
    Task<PrescriptionDto> Get(long id);
    Task<bool> MarkWarningAsSent(long id);
}