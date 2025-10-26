using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rise.Persistence;
using Rise.Persistence.Models.Identity;

namespace Rise.Services.Tests.Identity;

public static class FakeRoleManager
{
    public static RoleManager<ApplicationRole> Generate(ApplicationDbContext? dbContext = null)
    {
        var store = dbContext != null
            ? new RoleStore<ApplicationRole, ApplicationDbContext, Guid>(dbContext)
            : Substitute.For<IRoleStore<ApplicationRole>>();
        
        var roleValidators = new List<IRoleValidator<ApplicationRole>> { new RoleValidator<ApplicationRole>() };
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();
        var logger = Substitute.For<ILogger<RoleManager<ApplicationRole>>>();
        
        // Return a fully usable RoleManager
        return new RoleManager<ApplicationRole>(
            store,
            roleValidators,
            keyNormalizer,
            errors,
            logger
        );
    }
}