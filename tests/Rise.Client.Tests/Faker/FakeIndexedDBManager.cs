using Rise.Client.Offline;
using TG.Blazor.IndexedDB;

namespace Rise.Client.Faker;

public class FakeIndexedDbManager : IIndexedDbManager
{
    private readonly Dictionary<string, IndexedDbEntry> _cache = new();
    private Exception? _exceptionToThrow;

    public int OpenDbCallCount { get; private set; }
    public int AddRecordCallCount { get; private set; }
    public int UpdateRecordCallCount { get; private set; }
    public StoreRecord<IndexedDbEntry>? LastAddedRecord { get; private set; }
    public StoreRecord<IndexedDbEntry>? LastUpdatedRecord { get; private set; }

    public void SetRecord(string key, IndexedDbEntry record)
    {
        _cache[key] = record;
    }

    public Task OpenDb()
    {
        OpenDbCallCount++;

        if (_exceptionToThrow != null)
            throw _exceptionToThrow;

        return Task.CompletedTask;
    }

    public Task<IndexedDbEntry?> GetRecordById(string key)
    {
        _cache.TryGetValue(key, out var result);
        return Task.FromResult(result);
    }

    public Task AddRecord<TRecord>(StoreRecord<TRecord> recordToAdd)
    {
        AddRecordCallCount++;

        if (recordToAdd is StoreRecord<IndexedDbEntry> entry)
        {
            LastAddedRecord = entry;
            _cache[entry.Data.Key] = entry.Data;
        }

        return Task.CompletedTask;
    }

    public Task UpdateRecord<TRecord>(StoreRecord<TRecord> recordToUpdate)
    {
        UpdateRecordCallCount++;

        if (recordToUpdate is StoreRecord<IndexedDbEntry> entry)
        {
            LastUpdatedRecord = entry;
            _cache[entry.Data.Key] = entry.Data;
        }

        return Task.CompletedTask;
    }

    public string GetStoreName() => "FakeStore";
    
    public void ThrowOnOpenDb(Exception exception)
    {
        _exceptionToThrow = exception;
    }

    public void Clear()
    {
        _cache.Clear();
        _exceptionToThrow = null;
        OpenDbCallCount = 0;
        AddRecordCallCount = 0;
        UpdateRecordCallCount = 0;
        LastAddedRecord = null;
        LastUpdatedRecord = null;
    }
}
