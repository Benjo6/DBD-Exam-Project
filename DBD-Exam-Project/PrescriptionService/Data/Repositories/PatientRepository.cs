using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public class PatientRepository : BaseAsyncRepository<Patient>
{
    public PatientRepository(DbContext dbContext, DbSet<Patient> contextCollection) : base(dbContext, contextCollection)
    {
    }
}
