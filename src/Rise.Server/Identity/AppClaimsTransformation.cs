using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Rise.Persistence.Models.Identity;
using Rise.Shared.Identity;
using System.Security.Claims;

namespace Rise.Server.Identity;

public class AppClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMemoryCache _cache;

    public AppClaimsTransformation(
        UserManager<ApplicationUser> userManager,
        IMemoryCache cache)
    {
        _userManager = userManager;
        _cache = cache;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
            return principal;

        var oid = principal.GetOid();

        if (!Guid.TryParse(oid, out var oidGuid))
            return principal;

        var cacheKey = $"roles:{oid}";

        if (!_cache.TryGetValue(cacheKey, out List<string>? roles))
        {
            var appUser = await _userManager.Users
                .SingleOrDefaultAsync(u => u.SsoId == oidGuid);

            if (appUser == null)
                return principal;

            roles = [.. await _userManager.GetRolesAsync(appUser)];

            _cache.Set(cacheKey, roles,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(120) //2min
                });
        }

        if (roles == null)
            return principal;

        var originalIdentity = (ClaimsIdentity)principal.Identity;
        var newIdentity = new ClaimsIdentity(
            originalIdentity.Claims,
            originalIdentity.AuthenticationType,
            originalIdentity.NameClaimType,
            originalIdentity.RoleClaimType
        );

        var existingRoles = newIdentity
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToHashSet();

        foreach (var role in roles)
        {
            if (!existingRoles.Contains(role))
                newIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return new ClaimsPrincipal(newIdentity);
    }
}
