using Microsoft.AspNetCore.Identity;
using Rise.Domain.Identity;

namespace Rise.Persistence.Models.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    private ApplicationRole() {} 
    public ApplicationRole(string roleId, string name)
    {
        Id = new Guid(roleId);
        Name = name;
    }
    
    public Role MapToDomain()
    {
        return new Role(this.Id, this.Name!);
    }
}