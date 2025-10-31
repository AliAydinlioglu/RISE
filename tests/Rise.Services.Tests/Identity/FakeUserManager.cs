using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Rise.Persistence;
using Rise.Persistence.Models.Identity;

namespace Rise.Services.Tests.Identity;

public static class FakeUserManager 
{
    public static UserManager<ApplicationUser> Generate(
        ApplicationDbContext? dbContext = null, 
        Exception? exceptionWhenUpdating = null, 
        Exception? exceptionWhenCreating = null)
    {
        // store might be initialized for seeding it with data
        var store = dbContext != null
            ? new UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>(dbContext)
            : Substitute.For<IUserStore<ApplicationUser>>();
        
        var identityOptions = Options.Create(new IdentityOptions());
        var userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
        var pwdValidators = new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() };
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var logger = Substitute.For<ILogger<UserManager<ApplicationUser>>>();

        var userManager = Substitute.ForPartsOf<UserManager<ApplicationUser>>(
            store, 
            identityOptions, 
            passwordHasher, 
            userValidators, 
            pwdValidators,
            keyNormalizer, 
            errors, 
            null!, 
            logger
        );

        if (exceptionWhenCreating != null)
        {
            userManager.CreateAsync(Arg.Any<ApplicationUser>())
                .ThrowsAsync(exceptionWhenCreating);
        }

        if (exceptionWhenUpdating != null)
        {
            userManager.UpdateAsync(Arg.Any<ApplicationUser>())
                .ThrowsAsync(exceptionWhenUpdating);
        }
        
        return userManager;
    }
}