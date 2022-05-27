using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories
{
    public class PharmaceutRepository : BaseAsyncRepository<Pharmaceut>
    {
        public PharmaceutRepository(PostgresContext dbContext) : base(dbContext, dbContext.Pharmaceuts) { }

        protected override IQueryable<Pharmaceut> DefaultInclude()
        {
            return base.DefaultInclude()
                .Include(p=>p.PersonalData)
                .Include(p=>p.Pharmacy);
        }
    }
}
