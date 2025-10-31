using Microsoft.AspNetCore.Identity;

namespace Rise.Persistence.Models.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ClassGroup { get; private set; }
    public DateTimeOffset LastLogin { get; set; }
    public Guid SsoId { get; private set; }
    public string SsoProvider { get; private set; }
    
    public ApplicationUser(
        string email, 
        string? firstName, 
        string? lastName, 
        string? classGroup, 
        DateTimeOffset lastLogin, 
        Guid ssoId, 
        string ssoProvider)
    {
        UserName = email;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        ClassGroup = classGroup;
        LastLogin = lastLogin;
        SsoId = ssoId;
        SsoProvider = ssoProvider;
    }
}