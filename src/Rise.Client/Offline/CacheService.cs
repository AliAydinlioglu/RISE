using TG.Blazor.IndexedDB;

namespace Rise.Client.Offline;

public class CacheService(IIndexedDbManager dbManager) : ICacheService
{
    public async Task<string?> GetAsync(string key)
    {
        try
        {
            await dbManager.OpenDb();
            var entry = await dbManager.GetRecordById(key);
            return entry?.Data;
        }
        catch (Exception e)
        {
            Log.Error($"Fout bij het uitlezen van de cache: {e.Message}");
            return null;
        }
    }

    public async Task SetAsync(string key, string value)
    {
        try
        {
            await dbManager.OpenDb();
            await UpsertRecordAsync(key, value);
        }
        catch (Exception e)
        {
            Log.Error($"Fout bij het schrijven naar de cache: {e.Message}");
        }
    }

    private async Task UpsertRecordAsync(string key, string value)
    {
        var result = await dbManager.GetRecordById(key);
        var existingRecord = result?.Data != null;
        var entry = CreateStoreRecord(key, value);

        if (existingRecord)
        {
            await dbManager.UpdateRecord(entry);
            Log.Information($"Entry met key: {entry.Data.Key} werd succesvol bijgewerkt");
        }
        else
        {
            await dbManager.AddRecord(entry);
            Log.Information($"Entry met key: {entry.Data.Key} werd succesvol toegevoegd");
        }
    }

    private StoreRecord<IndexedDbEntry> CreateStoreRecord(string key, string value)
    {
        return new StoreRecord<IndexedDbEntry>
        {
            Storename = dbManager.GetStoreName(),
            Data = new IndexedDbEntry(key, value)
        };
    }
}

// TODO RI2526T2-13: is het interessant om een ttl te voorzien om te voorkomen dat de gebruiker extremisch oude data te zien krijgt?
public record IndexedDbEntry(string Key, string Data);