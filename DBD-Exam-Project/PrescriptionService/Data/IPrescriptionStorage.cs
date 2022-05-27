#nullable enable
using lib.DTO;
using PrescriptionService.Models;

namespace PrescriptionService.Data;

public interface IPrescriptionStorage
{
    Task<PrescriptionDto> Create(PrescriptionDto prescription);
    Task<PrescriptionDto> Update(PrescriptionDto prescription, int actorId);
    Task<IEnumerable<PrescriptionDto>> GetAll(bool expired = false, Page? pageInfo = null);
    Task<IEnumerable<PrescriptionDto>> GetAll(string cprNumber, Page? pageInfo = null);
    Task<PrescriptionDto> Get(long id);
}