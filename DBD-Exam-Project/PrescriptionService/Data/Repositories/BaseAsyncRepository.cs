using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data.Repositories;

public abstract class BaseAsyncRepository<T> : BaseAsyncRepository<T, int>, IAsyncRepository<T>
    where T : class, EntityWithId<int>
{
    protected BaseAsyncRepository(DbContext dbContext, DbSet<T> contextCollection) 
        : base(dbContext, contextCollection) { }
}

public abstract class BaseAsyncRepository<T, IdType> : IAsyncRepository<T, IdType> 
    where T : class, EntityWithId<IdType>
    where IdType : notnull
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> ContextCollection;

    protected BaseAsyncRepository(DbContext dbContext, DbSet<T> contextCollection)
    {
        Context = dbContext;
        ContextCollection = contextCollection;
    }

    public Task<T?> Get(IdType id)
        => DefaultInclude().FirstOrDefaultAsync(x => x.Id.Equals(id));
    

    public IAsyncEnumerable<T> GetAll()
        => DefaultInclude().AsAsyncEnumerable();

    public async Task<T> Create(T entity)
    {
        await ContextCollection.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        ContextCollection.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(IdType id)
    {
        T entity = await ContextCollection.FirstAsync(x => x.Id.Equals(id));
        ContextCollection.Remove(entity);
        await Context.SaveChangesAsync();
    }

    protected virtual IQueryable<T> DefaultInclude()
        => ContextCollection;
}
