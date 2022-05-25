namespace PrescriptionService.Data.Repositories;

public interface IAsyncRepository<T> : IAsyncRepository<T, int> { }

public interface IAsyncRepository<T, IdType>
{
    Task<T?> Get(IdType id);
    IAsyncEnumerable<T> GetAll();
    IQueryable<T> CustomQuery();
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task Delete(IdType id);
}

