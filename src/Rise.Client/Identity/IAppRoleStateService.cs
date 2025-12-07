namespace Rise.Client.Identity;

public interface IAppRoleStateService
{
    public event Func<Task>? RolesChanged;

    Task InitializeAsync();
    void SetRoles(IEnumerable<string>? roles);
    void ClearRoles();
    bool IsInRole(string role);
}

