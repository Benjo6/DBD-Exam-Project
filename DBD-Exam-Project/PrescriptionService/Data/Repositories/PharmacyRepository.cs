using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PharmacyRepository: BaseAsyncRepository<Pharmacy>
{
    public PharmacyRepository(DbContext dbContext, DbSet<Pharmacy> contextCollection) : base(dbContext, contextCollection)
    {
    }
}
