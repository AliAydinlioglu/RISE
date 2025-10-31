using Rise.Persistence.Models.Identity;
using Rise.Shared.Identity;

namespace Rise.Services.Tests.Identity;

public static class FakeApplicationUser
{
    public static ApplicationUser New(Guid ssoId) 
        => new("example@example.com", 
            "example", 
            "example", 
            null, 
            DateTimeOffset.UtcNow, 
            ssoId,
            SsoProviders.MicrosoftEntra);
}