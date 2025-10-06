namespace Rise.Shared.User;

public interface IUserRepository
{
    Task<string?> GetClassGroupAsync(string userId);
}