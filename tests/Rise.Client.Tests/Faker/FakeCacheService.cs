using Rise.Client.Offline;

namespace Rise.Client.Faker;

public class FakeCacheService : ICacheService
{
    private readonly Dictionary<string, string> _cache = new();
    
    public int GetAsyncCallCount { get; private set; }
    public int SetAsyncCallCount { get; private set; }
    public string? LastGetKey { get; private set; }
    public string? LastSetKey { get; private set; }
    public string? LastSetValue { get; private set; }

    public Task<string?> GetAsync(string key)
    {
        GetAsyncCallCount++;
        LastGetKey = key;
        return Task.FromResult(_cache.TryGetValue(key, out var value) ? value : null);
    }

    public Task SetAsync(string key, string value)
    {
        SetAsyncCallCount++;
        LastSetKey = key;
        LastSetValue = value;
        _cache[key] = value;
        return Task.CompletedTask;
    }

    public void SetCachedValue(string key, string value)
    {
        _cache[key] = value;
    }

    public void Clear()
    {
        _cache.Clear();
        GetAsyncCallCount = 0;
        SetAsyncCallCount = 0;
        LastGetKey = null;
        LastSetKey = null;
        LastSetValue = null;
    }
}