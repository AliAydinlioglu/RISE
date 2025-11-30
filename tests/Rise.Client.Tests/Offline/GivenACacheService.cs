using Rise.Client.Faker;
using Shouldly;

namespace Rise.Client.Offline;

public class GivenACacheService : IDisposable
{
    private const string StoreName = "FakeStore";
    
    private readonly FakeIndexedDbManager _fakeDbManager;
    private readonly CacheService _cacheService;

    public GivenACacheService()
    {
        _fakeDbManager = new FakeIndexedDbManager();
        _cacheService = new CacheService(_fakeDbManager);
    }

    [Theory(DisplayName = "When fetching a cached record, then the cached record should be returned")]
    [InlineData("test-key-1", "test-value-1")]
    [InlineData("https://api.example.com/users/123", "{\"id\":123,\"name\":\"John\"}")]
    [InlineData("complex/path?param=value", "<html>content</html>")]
    public async Task GetAsync_WhenRecordExists_ShouldReturnData(string key, string expectedData)
    {
        var entry = new IndexedDbEntry(key, expectedData);
        _fakeDbManager.SetRecord(key, entry);

        var result = await _cacheService.GetAsync(key);

        result.ShouldBe(expectedData);
        _fakeDbManager.GetStoreName().ShouldBe(StoreName);
        _fakeDbManager.OpenDbCallCount.ShouldBe(1);
    }

    [Fact(DisplayName = "When fetching a non-cached record, then null should be returned")]
    public async Task GetAsync_WhenRecordDoesNotExist_ShouldReturnNull()
    {
        var key = "non-existent-key";

        var result = await _cacheService.GetAsync(key);

        result.ShouldBeNull();
    }

    [Fact(DisplayName = "When an error occurs, then null should be returned")]
    public async Task GetAsync_WhenExceptionOccurs_ShouldReturnNull()
    {
        _fakeDbManager.ThrowOnOpenDb(new Exception("Database error"));

        var result = await _cacheService.GetAsync("error-key");

        result.ShouldBeNull();
    }

    [Fact(DisplayName = "When saving a non-cached entry, then a new entry should be stored in db")]
    public async Task SetAsync_WhenRecordIsNew_ShouldAddRecord()
    {
        var key = "new-key";
        var value = "new-value";

        await _cacheService.SetAsync(key, value);

        _fakeDbManager.OpenDbCallCount.ShouldBe(1);
        _fakeDbManager.AddRecordCallCount.ShouldBe(1);
        _fakeDbManager.UpdateRecordCallCount.ShouldBe(0);
        
        var addedRecord = _fakeDbManager.LastAddedRecord;
        addedRecord.ShouldNotBeNull();
        addedRecord.Storename.ShouldBe(StoreName);
        addedRecord.Data.Key.ShouldBe(key);
        addedRecord.Data.Data.ShouldBe(value);
    }

    [Fact(DisplayName = "When saving a cached entry, then the saved entry should be updated in db")]
    public async Task SetAsync_WhenRecordExists_ShouldUpdateRecord()
    {
        var key = "existing-key";
        var oldValue = "old-value";
        var newValue = "new-value";
        var existingEntry = new IndexedDbEntry(key, oldValue);

        _fakeDbManager.SetRecord(key, existingEntry);

        await _cacheService.SetAsync(key, newValue);

        _fakeDbManager.UpdateRecordCallCount.ShouldBe(1);
        _fakeDbManager.AddRecordCallCount.ShouldBe(0);
        
        var updatedRecord = _fakeDbManager.LastUpdatedRecord;
        updatedRecord.ShouldNotBeNull();
        updatedRecord.Storename.ShouldBe(StoreName);
        updatedRecord.Data.Key.ShouldBe(key);
        updatedRecord.Data.Data.ShouldBe(newValue);
    }
    
    public void Dispose()
    {
        _fakeDbManager.Clear();
    }
}