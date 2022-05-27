using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PharmaceutRepository: BaseAsyncRepository<Pharmaceut>
{
    public PharmaceutRepository(DbContext dbContext, DbSet<Pharmaceut> contextCollection) 
        : base(dbContext, contextCollection) { }

    protected override IQueryable<Pharmaceut> DefaultInclude()
        => base.DefaultInclude()
            .Include(x => x.PersonalData)
            .Include(x => x.PersonalData.Address);
}
