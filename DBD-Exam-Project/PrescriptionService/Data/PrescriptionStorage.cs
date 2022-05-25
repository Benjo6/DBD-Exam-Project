using lib.DTO;
using lib.Models;
using PrescriptionService.DAP;

namespace PrescriptionService.Data;

class PrescriptionStorage : IPrescriptionStorage
{
    private readonly IPrescriptionRepo _database;
    private readonly IPrescriptionCache _cache;

    public PrescriptionStorage(IPrescriptionRepo database, IPrescriptionCache cache)
    {
        _database = database;
        _cache = cache;
    }

    public async Task<PrescriptionDto> Create(PrescriptionDto prescription, Doctor doctor)
    {
        throw new NotImplementedException();
    }

    public PrescriptionDto Update(PrescriptionDto prescription)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PrescriptionDto> GetAll(bool expired = false)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PrescriptionDto> GetAll(int userId, bool expired = false)
    {
        throw new NotImplementedException();
    }
}