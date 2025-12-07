namespace Rise.Client.Identity;

public interface IAppRoleStateService
{
    public event Func<Task>? RolesChanged;

    Task InitializeAsync();
    Task SetRolesAsync(IEnumerable<string>? roles);
    Task ClearRolesAsync();
    bool IsInRole(string role);
}

