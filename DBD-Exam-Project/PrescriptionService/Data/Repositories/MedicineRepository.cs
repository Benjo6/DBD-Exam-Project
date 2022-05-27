using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class MedicineRepository: BaseAsyncRepository<Medicine>
{
    public MedicineRepository(DbContext dbContext, DbSet<Medicine> contextCollection) 
        : base(dbContext, contextCollection) { }
}
