using Microsoft.AspNetCore.Components;
using Rise.Client.Offline;

namespace Rise.Client.Layout;

public partial class OfflineBanner: IDisposable
{
    [Inject] public required IConnectionService ConnectionService { get; set; }
    
    private bool _isOnline = true;

    protected override async Task OnInitializedAsync()
    {
        await ConnectionService.InitializeAsync();
        _isOnline = ConnectionService.IsOnline;
        ConnectionService.ConnectionStateChanged += OnConnectionStatusChanged;
    }

    private void OnConnectionStatusChanged(bool isOnline)
    {
        _isOnline = isOnline;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ConnectionService.ConnectionStateChanged -= OnConnectionStatusChanged;
    }
}