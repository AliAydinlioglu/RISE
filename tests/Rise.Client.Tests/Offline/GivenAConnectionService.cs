using Rise.Client.Faker;
using Shouldly;

namespace Rise.Client.Offline;

public class GivenAConnectionService : IDisposable
{
    private readonly FakeJsRuntime _fakeJsRuntime;
    private readonly ConnectionService _connectionService;

    public GivenAConnectionService()
    {
        _fakeJsRuntime = new FakeJsRuntime();
        _connectionService = new ConnectionService(_fakeJsRuntime);
    }

    [Fact(DisplayName = "When initialized, then JavaScript Initialize should be called")]
    public async Task InitializeAsync_ShouldCallJavaScriptInitialize()
    {
        await _connectionService.InitializeAsync();

        _fakeJsRuntime.InvokeAsyncCallCount.ShouldBe(1);
        _fakeJsRuntime.LastInvokedFunction.ShouldBe("Connection.Initialize");
        _fakeJsRuntime.LastInvokedArgs.ShouldNotBeNull();
        _fakeJsRuntime.LastInvokedArgs!.Length.ShouldBe(1);
    }

    [Fact(DisplayName = "When connection status changes to offline, then ConnectionStateChanged event should be triggered containing the correct value")]
    public void OnConnectionStatusChanged_ShouldTriggerEvent()
    {
        bool? eventResult = null;
        _connectionService.ConnectionStateChanged += (isOnline) => eventResult = isOnline;

        _connectionService.OnConnectionStatusChanged(false);

        eventResult.ShouldNotBeNull();
        eventResult.Value.ShouldBeFalse();
        _connectionService.IsOnline.ShouldBeFalse();
    }

    [Fact(DisplayName = "When connection status changes to online, then ConnectionStateChanged event should be triggered containing the correct value")]
    public void OnConnectionStatusChanged_EventShouldContainCorrectValue()
    {
        bool? eventResult = null;
        _connectionService.ConnectionStateChanged += (isOnline) => eventResult = isOnline;

        _connectionService.OnConnectionStatusChanged(true);

        eventResult.ShouldNotBeNull();
        eventResult.Value.ShouldBeTrue();
        _connectionService.IsOnline.ShouldBeTrue();
    }
    
    [Fact(DisplayName = "When disposed, then JavaScript Dispose should be called")]
    public async Task DisposeAsync_ShouldCallJavaScriptDispose()
    {
        await _connectionService.InitializeAsync();
        _fakeJsRuntime.Reset();

        await _connectionService.DisposeAsync();

        _fakeJsRuntime.InvokeAsyncCallCount.ShouldBe(1);
        _fakeJsRuntime.LastInvokedFunction.ShouldBe("Connection.Dispose");
    }
    
    public void Dispose()
    {
        _fakeJsRuntime.Reset();
        _connectionService.DisposeAsync().AsTask().Wait();
    }
}