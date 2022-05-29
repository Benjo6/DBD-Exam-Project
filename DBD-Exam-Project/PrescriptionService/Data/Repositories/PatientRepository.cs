#nullable enable
using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public interface IPatientRepository : IAsyncRepository<Patient>
{
    Task<Patient?> GetByCpr(string cpr);
}

public class PatientRepository : BaseAsyncRepository<Patient>, IPatientRepository
{
    public PatientRepository(PostgresContext dbContext) : base(dbContext, dbContext.Patients) { }

    protected override IQueryable<Patient> DefaultInclude()
        => base.DefaultInclude()
            .Include(x => x.PersonalData)
            .Include(x => x.PersonalData.Address);

    public Task<Patient?> GetByCpr(string cpr)
        => DefaultInclude().FirstOrDefaultAsync(x => x.Cpr == cpr);
}
