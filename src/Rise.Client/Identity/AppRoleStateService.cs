using Microsoft.JSInterop;
using System.Text.Json;

namespace Rise.Client.Identity;

public class AppRoleStateService(IJSRuntime jsRuntime) : IAppRoleStateService
{
    private IReadOnlyList<string> _roles = [];
    public event Func<Task>? RolesChanged;

    public IReadOnlyList<string> Roles => _roles;
    public bool HasRoles => _roles?.Count > 0;

    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public async Task InitializeAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "roles");
        if (!string.IsNullOrEmpty(json))
        {
            var roles = JsonSerializer.Deserialize<List<string>>(json);
            await SetRolesAsync(roles);
        }
    }

    public async Task SetRolesAsync(IEnumerable<string>? roles)
    {
        _roles = (roles ?? Array.Empty<string>()).ToArray();
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "roles", JsonSerializer.Serialize(roles));
        _ = NotifyChanged();
    }

    public async Task ClearRolesAsync()
    {
        _roles = Array.Empty<string>();
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "roles");
        _ = NotifyChanged();
    }

    public bool IsInRole(string role) =>
        _roles?.Contains(role, StringComparer.OrdinalIgnoreCase) ?? false;

    private Task NotifyChanged()
    {
        var ev = RolesChanged;
        return ev == null ? Task.CompletedTask : ev.Invoke();
    }
}

