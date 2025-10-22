namespace Rise.Domain.Identity;

public class User : Entity<Guid>
{
    public string Email { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ClassGroup { get; private set; }

    private User(){}
    
    public User(string email, string? firstName, string? lastName, string? classGroup)
    {
        Email = Guard.Against.InvalidEmailFormat(email);
        FirstName = firstName;
        LastName = lastName;
        ClassGroup = classGroup;
    }
}