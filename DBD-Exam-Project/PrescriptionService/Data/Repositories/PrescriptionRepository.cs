using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public interface IPrescriptionRepository: IAsyncRepository<Prescription, long>
{
    IAsyncEnumerable<Prescription> GetAllExpired();
    IAsyncEnumerable<Prescription> GetAllForPatient(string cprNumber);
    IAsyncEnumerable<Prescription> GetAllForDoctor(int doctorId);
    Task<Prescription> GetDetailed(long id);
}

public class PrescriptionRepository: BaseAsyncRepository<Prescription, long>, IPrescriptionRepository
{
    public PrescriptionRepository(PostgresContext dbContext) 
        : base(dbContext, dbContext.Prescriptions) { }

    public IAsyncEnumerable<Prescription> GetAllExpired()
        => DefaultInclude()
            .Include(x => x.PrescribedToNavigation)
            .Include(x => x.PrescribedToNavigation.PersonalData)
            .Where(x => x.Expiration < DateTime.Now.AddDays(7))
            .OrderByDescending(x => x.Expiration)
            .AsAsyncEnumerable();

    public IAsyncEnumerable<Prescription> GetAllForPatient(string cprNumber)
        => DefaultInclude()
            .Where(x => x.PrescribedToCpr == cprNumber)
            .AsAsyncEnumerable();

    public IAsyncEnumerable<Prescription> GetAllForDoctor(int doctorId)
        => DefaultInclude()
            .Include(x => x.PrescribedToNavigation)
            .Where(x => x.PrescribedBy == doctorId)
            .AsAsyncEnumerable();

    public Task<Prescription?> GetDetailed(long id)
        => DefaultInclude()
            .Include(x => x.PrescribedToNavigation)
            .Include(x => x.PrescribedToNavigation.PersonalData)
            .Include(x => x.PrescribedByNavigation)
            .Include(x => x.PrescribedByNavigation.PersonalData)
            .FirstOrDefaultAsync(x => x.Id == id);


    protected override IQueryable<Prescription> DefaultInclude()
        => base.DefaultInclude().Include(x => x.Medicine);
    
}
