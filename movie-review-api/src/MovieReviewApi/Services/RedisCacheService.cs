using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public T? Get<T>(string key)
    {
        try
        {
            var db = _connectionMultiplexer.GetDatabase(); // Redis veritabanını al
            var cachedData = db.StringGet(key); // Redis'ten veriyi al
            if (cachedData.IsNullOrEmpty)
            {
                return default; // Veri yoksa varsayılan değeri döndür
            }
            return JsonSerializer.Deserialize<T>(cachedData!); // Veriyi deserialize et
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Redis Get Error: {ex.Message}");
            return default;
        }
    }

    public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
    {
        try
        {
            var db = _connectionMultiplexer.GetDatabase(); // Redis veritabanını al
            var serializedData = JsonSerializer.Serialize(value); // Veriyi serialize et
            db.StringSet(key, serializedData, absoluteExpiration); // Redis'e kaydet
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Redis Set Error: {ex.Message}");
        }
    }

    public void Remove(string key)
    {
        try
        {
            var db = _connectionMultiplexer.GetDatabase(); // Redis veritabanını al
            db.KeyDelete(key); // Redis'ten anahtarı sil
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Redis Remove Error: {ex.Message}");
        }
    }
}