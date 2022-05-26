using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PharmacyRepository: BaseAsyncRepository<Pharmacy>
{
    public PharmacyRepository(PostgresContext dbContext) 
        : base(dbContext, dbContext.Pharmacies) { }

    protected override IQueryable<Pharmacy> DefaultInclude()
        => base.DefaultInclude()
            .Include(x => x.Address);
}
