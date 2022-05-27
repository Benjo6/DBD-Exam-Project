using System.ComponentModel.DataAnnotations;

namespace lib.Configurations;

public class RedisCacheConfig
{
    public const string ConfigKey = "RedisConfiguration";
    [Required]
    public string EndPoints { get; set; }
    public int RetentionTimeSec { get; set; } = 0;
    public int BulkRetentionTimeSec { get; set; } = 0;
}
