#nullable enable
using lib.Configurations;
using lib.Models;
using MessagePack;
using Microsoft.Extensions.Options;
using PrescriptionService.Models;
using StackExchange.Redis;

namespace PrescriptionService.Data;

public class RedisCache: IRedisCache
{
    private readonly IConnectionMultiplexer _connection;
    private readonly TimeSpan? _retentionTimeSec;
    private readonly TimeSpan? _bulkRetentionTimeSec;

    public RedisCache(IConnectionMultiplexer connection, IOptionsSnapshot<RedisCacheConfig> redisConfig)
    {
        _connection = connection;
        _retentionTimeSec = redisConfig.Value.RetentionTimeSec > 0 ? TimeSpan.FromSeconds(redisConfig.Value.RetentionTimeSec) : null;
        _bulkRetentionTimeSec = redisConfig.Value.BulkRetentionTimeSec > 0 ? TimeSpan.FromSeconds(redisConfig.Value.BulkRetentionTimeSec) : null;
    }

    public async Task Store<TSource, TKey>(TSource source) 
        where TSource : class, EntityWithId<TKey>
        where TKey : notnull
        => await Store(source, source.Id);
    

    public async Task Store<TSource, TKey>(TSource source, TKey keyValue) 
        where TSource : notnull
        where TKey : notnull
    {
        IDatabase db = _connection.GetDatabase();
        byte[] messagePack = MessagePackSerializer.Serialize(source);
        RedisKey key = new($"model:{source.GetType().Name}:{keyValue}");
        await db.StringSetAsync(key, messagePack, _retentionTimeSec);
    }

    public async Task BulkStore<TSource, TKey>(IEnumerable<TSource> source, string bulkKey) where TSource : class, EntityWithId<TKey> where TKey : notnull
    {
        IDatabase db = _connection.GetDatabase();
        string key = $"{source.GetType().GenericTypeArguments.First().Name}:{bulkKey}";

        byte[] messagePack = MessagePackSerializer.Serialize(source);
        RedisKey serializeKey = new($"bulk:{key}");
        await db.StringSetAsync(serializeKey, messagePack, _bulkRetentionTimeSec);

        RedisValue[] ids = source
            .Select(x => new RedisValue(x.Id.ToString()))
            .ToArray();
        RedisKey setKey = new($"idList:{key}");
        await db.KeyDeleteAsync(setKey);
        await db.SetAddAsync(setKey, ids);
    }

    public async Task<IEnumerable<TResult>> BulkRetrive<TResult, TKey>(string bulkKey) where TResult : class, EntityWithId<TKey>
    {
        IDatabase db = _connection.GetDatabase();

        RedisKey key = new($"bulk:{typeof(TResult).Name}:{bulkKey}");
        RedisValue data = await db.StringGetAsync(key);

        return data.HasValue
            ? MessagePackSerializer.Deserialize<IEnumerable<TResult>>((byte[])data)
            : Enumerable.Empty<TResult>();
    }

    public async Task<TResult?> Retrive<TResult, TKey>(TKey id) where TResult : class where TKey : notnull
    {
        IDatabase db = _connection.GetDatabase();
        RedisKey key = new($"model:{typeof(TResult).Name}:{id}");
        RedisValue data = await db.StringGetAsync(key);

        return data.HasValue
            ? MessagePackSerializer.Deserialize<TResult>((byte[])data)
            : null;
    }

    public async Task<bool> ExistsInBulk<TSource, TKey>(TKey id, string bulkKey) where TKey : notnull
    {
        IDatabase db = _connection.GetDatabase();

        RedisKey setKey = new($"idList:{typeof(TSource).Name}:{bulkKey}");
        return await db.SetContainsAsync(setKey, id.ToString());
    }

    public async Task ClearBulk<TSource>(string bulkKey)
    {
        IDatabase db = _connection.GetDatabase();

        string key = $"{typeof(TSource).Name}:{bulkKey}";
        RedisKey[] keys = {
            new($"bulk:{key}"),
            new($"idList:{key}")
        };
        await db.KeyDeleteAsync(keys);
    }
}
