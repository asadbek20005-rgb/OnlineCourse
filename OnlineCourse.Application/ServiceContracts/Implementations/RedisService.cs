
using Newtonsoft.Json;
using StackExchange.Redis;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class RedisService(IConnectionMultiplexer connection) : IRedisService
{
    private readonly IDatabase _database = connection.GetDatabase();
    public async Task<TResponse?> GetAsync<TResponse>(string key)
    {
        RedisValue redisValue = await _database.StringGetAsync(key);

        if (!redisValue.HasValue)
            return default;


        var item = JsonConvert.DeserializeObject<TResponse>(redisValue);

        return item;
    }

    public async Task SetAsync<T>(string key, T item)
    {
        var json = JsonConvert.SerializeObject(item);

        await _database.StringSetAsync(key, json, TimeSpan.FromMinutes(1));
    }
}


/*
 * public class RedisService(IConnectionMultiplexer connectionMultiplexer) : IRedisService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    public async Task<T?> GetItemAsync<T>(string key)
    {
        string? json = await _database.StringGetAsync(key);
        if (json is null)
            return default;
        var item = JsonConvert.DeserializeObject<T>(json);
        
        return item;
    }

    public async Task SetItemAsync<T>(string key, T item)
    {
        string json = JsonConvert.SerializeObject(item);
        await _database.StringSetAsync(key, json, TimeSpan.FromMinutes(2));
    }
 */