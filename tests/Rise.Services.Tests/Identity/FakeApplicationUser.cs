using Rise.Persistence.Models.Identity;
using Rise.Shared.Identity;

namespace Rise.Services.Tests.Identity;

public static class FakeApplicationUser
{
    public static ApplicationUser Generate(Guid ssoId)
    {
        return new ApplicationUser(
            "example@example.com", 
            "example", 
            "example", 
            null, 
            DateTimeOffset.UtcNow, 
            ssoId,
            SsoProviders.MicrosoftEntra);
    }
}