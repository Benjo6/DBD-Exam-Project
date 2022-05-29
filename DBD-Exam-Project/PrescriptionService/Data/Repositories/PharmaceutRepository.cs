using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PharmaceutRepository: BaseAsyncRepository<Pharmaceut>
{
    public PharmaceutRepository(PostgresContext dbContext) 
        : base(dbContext, dbContext.Pharmaceuts) { }

    protected override IQueryable<Pharmaceut> DefaultInclude()
        => base.DefaultInclude()
            .Include(x => x.PersonalData)
            .Include(x => x.PersonalData.Address);
}
