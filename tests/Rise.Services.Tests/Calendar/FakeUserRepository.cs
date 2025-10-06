using Rise.Shared.User;

namespace Rise.Services.Tests.Calendar;

public class FakeUserRepository: IUserRepository
{
    private readonly Dictionary<string, string> _users = new();

    public void AddUser(string userId, string classGroup)
    {
        _users[userId] = classGroup;
    }
    
    public Task<string?> GetClassGroupAsync(string userId)
    {
        return Task.FromResult(_users.TryGetValue(userId, out var classGroup) ? classGroup : null);
    }
}