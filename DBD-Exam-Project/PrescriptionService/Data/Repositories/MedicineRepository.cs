#nullable enable
using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public interface IMedicineRepository: IAsyncRepository<Medicine>
{
    Task<Medicine?> Get(string name);
}

public class MedicineRepository: BaseAsyncRepository<Medicine>, IMedicineRepository
{
    public MedicineRepository(PostgresContext dbContext) 
        : base(dbContext, dbContext.Medicines) { }

    public Task<Medicine?> Get(string name)
        => DefaultInclude()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
}
