using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PatientRepository : BaseAsyncRepository<Patient>
{
    public PatientRepository(PostgresContext dbContext) : base(dbContext, dbContext.Patients)
    {
    }
}
