#nullable enable
using lib.Models;
using PrescriptionService.Models;

namespace PrescriptionService.Data;

public interface IRedisCache
{
    Task Store<TSource, TKey>(TSource source) where TSource : class, EntityWithId<TKey>;
    Task BulkStore<TSource, TKey>(IEnumerable<TSource> source, string bulkKey)
        where TSource : class, EntityWithId<TKey>
        where TKey : notnull;
    Task<IEnumerable<TResult>> BulkRetrive<TResult, TKey>(string bulkKey)
        where TResult: class, EntityWithId<TKey>;
    Task<TResult?> Retrive<TResult, TKey>(TKey id) 
        where TResult : class, EntityWithId<TKey>
        where TKey : notnull;
    Task<bool> ExistsInBulk<TSource, TKey>(TKey id, string bulkKey)
        where TKey : notnull;
    Task ClearBulk<TSource>(string bulkKey);
}
