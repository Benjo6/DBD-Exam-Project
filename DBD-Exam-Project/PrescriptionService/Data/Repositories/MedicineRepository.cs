using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories
{
    public class MedicineRepository: BaseAsyncRepository<Medicine>
    {
        public MedicineRepository(PostgresContext dbContext) : base(dbContext, dbContext.Medicines) { }

        protected override IQueryable<Medicine> DefaultInclude()
            => base.DefaultInclude()
            ;
    }
}
