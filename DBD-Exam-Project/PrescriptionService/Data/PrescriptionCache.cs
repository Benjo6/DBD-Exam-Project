using lib.Configurations;
using lib.Models;
using MessagePack;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace PrescriptionService.Data;

public class PrescriptionCache: IPrescriptionCache
{
    private readonly ConnectionMultiplexer _connection;
    private readonly TimeSpan? _retentionTimeSec;

    public PrescriptionCache(ConnectionMultiplexer connection, IOptionsSnapshot<RedisCacheConfig> redisConfig)
    {
        _connection = connection;
        _retentionTimeSec = redisConfig.Value.RetentionTimeSec > 0 ? TimeSpan.FromSeconds(redisConfig.Value.RetentionTimeSec) : null;
    }
    public async Task StorePrescription(Prescription prescription)
    {
        IDatabase db = _connection.GetDatabase();
        byte[] messagePack = MessagePackSerializer.Serialize(prescription);
        RedisKey key = new($"prescription:{prescription.Id}");
        await db.StringSetAsync(key, messagePack, _retentionTimeSec);
    }

    public async Task<Prescription> RetrivePrescription(long id)
    {
        IDatabase db = _connection.GetDatabase();
        RedisKey key = new($"prescription:{id}");
        byte[]? data = await db.StringGetAsync(key);

        return data is null ? null : MessagePackSerializer.Deserialize<Prescription>(data);
    }
}
