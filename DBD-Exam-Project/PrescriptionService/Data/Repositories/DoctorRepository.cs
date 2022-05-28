using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class DoctorRepository : BaseAsyncRepository<Doctor>
{
    public DoctorRepository(PostgresContext dbContext) 
        : base(dbContext, dbContext.Doctors) { }

    protected override IQueryable<Doctor> DefaultInclude()
        => base.DefaultInclude()
            .Include(x => x.PersonalData)
            .Include(x => x.PersonalData.Address);
}
