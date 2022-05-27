using lib.Models;
using Microsoft.EntityFrameworkCore;
using PrescriptionService.Data.Repositories;

namespace PrescriptionService.Data;

public interface IPrescriptionRepository: IAsyncRepository<Prescription, long>
{
    IAsyncEnumerable<Prescription> GetAllExpired();
    IAsyncEnumerable<Prescription> GetAllForPatient(string cprNumber);
}

public class PrescriptionRepository: BaseAsyncRepository<Prescription, long>, IPrescriptionRepository
{
    public PrescriptionRepository(PostgresContext dbContext) 
        : base(dbContext, dbContext.Prescriptions) { }

    public IAsyncEnumerable<Prescription> GetAllExpired()
        => DefaultInclude()
            .Include(x => x.PrescribedToNavigation)
            .Include(x => x.PrescribedToNavigation.PersonalData)
            .Include(x => x.Medicine)
            .Where(x => x.Expiration < DateTime.Now.AddDays(7))
            .OrderByDescending(x => x.Expiration)
            .AsAsyncEnumerable();

    public IAsyncEnumerable<Prescription> GetAllForPatient(string cprNumber)
        => DefaultInclude()
            .Include(x => x.Medicine)
            .Where(x => x.PrescribedToCpr == cprNumber)
            .AsAsyncEnumerable();

}
