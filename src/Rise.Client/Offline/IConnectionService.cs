// https://blog.rufer.be/2024/10/02/howto-implement-offline-capability-for-blazor-pwa-using-service-worker-and-local-storage/
using Microsoft.JSInterop;

namespace Rise.Client.Offline;

public interface IConnectionService
{
    bool IsOnline { get; }
    event Action<bool> ConnectionStateChanged;
    Task InitializeAsync();
}

public class ConnectionService(IJSRuntime jsRuntime) : IConnectionService, IAsyncDisposable
{
    // referentie om te voorkomen dat garbage collector deze instance opruimt
    private DotNetObjectReference<ConnectionService> _dotNetReference;

    public event Action<bool>? ConnectionStateChanged;
    public bool IsOnline { get; private set; }

    public async Task InitializeAsync()
    {
        _dotNetReference = DotNetObjectReference.Create(this);
        await jsRuntime.InvokeVoidAsync("Connection.Initialize", _dotNetReference);
    }
    
    [JSInvokable("Connection.StatusChanged")]
    public void OnConnectionStatusChanged(bool isOnline)
    {
        IsOnline = isOnline;
        ConnectionStateChanged?.Invoke(isOnline);
    }

    public async ValueTask DisposeAsync()
    {
        if (_dotNetReference != null)
        {
            await jsRuntime.InvokeVoidAsync("Connection.Dispose");
            _dotNetReference.Dispose();
        }
    }
}