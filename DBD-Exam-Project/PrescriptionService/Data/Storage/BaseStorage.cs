using AutoMapper;
using lib.Models;
using PrescriptionService.Models;

namespace PrescriptionService.Data.Storage;

public class BaseStorage<TDto, TDbModel>: BaseStorage<TDto, TDbModel, int>
    where TDbModel : class, EntityWithId<int>
{
    public BaseStorage(IRedisCache cache, IMapper mapper) 
        : base(cache, mapper) { }
}

public class BaseStorage<TDto, TDbModel, TKey>
    where TDbModel : class, EntityWithId<TKey>
    where TKey : notnull
{
    protected readonly IRedisCache Cache;
    protected readonly IMapper Mapper;

    public BaseStorage(IRedisCache cache, IMapper mapper)
    {
        Cache = cache;
        Mapper = mapper;
    }

    protected async Task<IEnumerable<TDto>> GetAll(IAsyncEnumerable<TDbModel> initalResult, string bulkKey, Page pageInfo)
    {
        IEnumerable<TDbModel> cachedData = await Cache.BulkRetrive<TDbModel, TKey>(bulkKey);
        if (cachedData.Any())
            return cachedData.Select(Mapper.Map<TDbModel, TDto>);

        List<TDbModel> data = await initalResult
            .Skip(pageInfo.Number * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        await Cache.BulkStore<TDbModel, TKey>(data, bulkKey);
        return data.Select(Mapper.Map<TDbModel, TDto>);
    }
}
