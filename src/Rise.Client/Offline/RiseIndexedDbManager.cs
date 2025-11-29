using TG.Blazor.IndexedDB;

namespace Rise.Client.Offline;

public class RiseIndexedDbManager(IndexedDBManager dbManager) : IIndexedDbManager
{
    private const string StoreName = "RiseOfflineCache";
    
    public Task OpenDb() => dbManager.OpenDb();
    public Task<IndexedDbEntry?> GetRecordById(string key) => dbManager.GetRecordById<string, IndexedDbEntry>(StoreName, key);
    public Task AddRecord<TRecord>(StoreRecord<TRecord> recordToAdd) => dbManager.AddRecord(recordToAdd);
    public Task UpdateRecord<TRecord>(StoreRecord<TRecord> recordToUpdate) => dbManager.UpdateRecord(recordToUpdate);
    public string GetStoreName() => StoreName;
}