using TG.Blazor.IndexedDB;

namespace Rise.Client.Offline;

public interface IIndexedDbManager
{
    Task OpenDb();
    Task<IndexedDbEntry?> GetRecordById(string key);
    Task AddRecord<TRecord>(StoreRecord<TRecord> recordToAdd);
    Task UpdateRecord<TRecord>(StoreRecord<TRecord> recordToUpdate);
    string GetStoreName();
}