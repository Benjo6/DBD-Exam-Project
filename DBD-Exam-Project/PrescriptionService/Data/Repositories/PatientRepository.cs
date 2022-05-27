using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PatientRepository : BaseAsyncRepository<Patient>
{
    public PatientRepository(PostgresContext dbContext) : base(dbContext, dbContext.Patients) { }

    protected override IQueryable<Patient> DefaultInclude()
        => base.DefaultInclude()
            .Include(x => x.PersonalData)
            .Include(x => x.PersonalData.Address);
}
