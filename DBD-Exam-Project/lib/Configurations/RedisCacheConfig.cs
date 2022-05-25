namespace lib.Configurations;

public class RedisCacheConfig
{
    public const string ConfigKey = "RedisConfiguration";
    public string EndPoints { get; set; }
    public int RetentionTimeSec { get; set; }
}
