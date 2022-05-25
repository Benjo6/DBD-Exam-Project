using lib.Models;
using Microsoft.EntityFrameworkCore;
using PrescriptionService.Data.Repositories;

namespace PrescriptionService.Data;

public interface IPrescriptionRepository: IAsyncRepository<Prescription, long>
{
    IEnumerable<Prescription> GetAllExpired();
    IEnumerable<Prescription> GetAllForPatient(string cprNumber);
}

public class PrescriptionRepository: BaseAsyncRepository<Prescription, long>, IPrescriptionRepository
{
    public PrescriptionRepository(DbContext dbContext, DbSet<Prescription> contextCollection) 
        : base(dbContext, contextCollection) { }

    public IEnumerable<Prescription> GetAllExpired()
        => ContextCollection
            .Include(x => x.PrescribedToNavigation)
            .Include(x => x.PrescribedToNavigation.PersonalData)
            .Include(x => x.Medicine)
            .Where(x => x.Expiration < DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            .OrderByDescending(x => x.Expiration)
            .AsEnumerable();

    public IEnumerable<Prescription> GetAllForPatient(string cprNumber)
        => ContextCollection
            .Include(x => x.Medicine)
            .Where(x => x.PrescribedToCpr == cprNumber)
            .AsEnumerable();
}
