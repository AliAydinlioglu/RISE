using Rise.Shared.User;

namespace Rise.Services.User;

public class DummyUserService: IUserRepository
{
    private readonly Dictionary<string, string> _users = new();

    public void AddUser(string userId, string classGroup)
    {
        _users[userId] = classGroup;
    }
    
    public Task<string?> GetClassGroupAsync(string userId)
    {
        return Task.FromResult("42")!;
    }
}