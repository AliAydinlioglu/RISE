using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Rise.Persistence;
using Rise.Persistence.Models.Identity;
using Rise.Services.Identity;
using Rise.Shared.Identity;
using Rise.TestDoubles.Fakers;

namespace Rise.Services.Tests.Identity;

public class UserServiceShould
{   
    [Fact]
    public async Task ReturnPublicRole_WhenUserIsUnknown()
    {
        // Arrange
        var roleManager = CreateRoleManager();
        var userManager = CreateUserManager();
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());

        // Act
        var result = await userService.GetRoleIdAsync(null);

        // Assert
        result.ShouldBe(new Guid(AppRoles.Public));
    }
    
    [Fact]
    public async Task ReturnPublicRole_WhenUserHasNoRole()
    {
        // Arrange
        var roleManager = CreateRoleManager();
        var userManager = CreateUserManager();
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([], "mock"));

        // Act
        var result = await userService.GetRoleIdAsync(claimsPrincipal);

        // Assert
        result.ShouldBe(new Guid(AppRoles.Public));
    }
    
    [Fact]
    public async Task ReturnSpecificRole_WhenUserHasRole()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(ReturnSpecificRole_WhenUserHasRole)) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
        
        using var dbContext = new ApplicationDbContext(options);
        
        var store = new RoleStore<ApplicationRole, ApplicationDbContext, Guid>(dbContext);
        var roleManager = new RoleManager<ApplicationRole>(
            store,
            new[] { new RoleValidator<ApplicationRole>() },
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            Substitute.For<ILogger<RoleManager<ApplicationRole>>>()
        );

        var userManager = CreateUserManager();

        var role = new ApplicationRole(AppRoles.RegularStudent, "Regular");
        await roleManager.CreateAsync(role);
        
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, "Regular")], "mock"));

        // Act
        var result = await userService.GetRoleIdAsync(claimsPrincipal);

        // Assert
        result.ShouldBe(new Guid(AppRoles.RegularStudent));
    }
    
    /// <summary>
    /// TODO: when this is needed in other tests => put in shared library 
    /// </summary>
    /// <returns></returns>
    private static RoleManager<ApplicationRole> CreateRoleManager()
    {
        var store = Substitute.For<IRoleStore<ApplicationRole>>();
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
    
    /// <summary>
    /// TODO: when this is needed in other tests => put in shared library 
    /// </summary>
    /// <returns></returns>
    private static UserManager<ApplicationUser> CreateUserManager()
    {
        var store = Substitute.For<IUserStore<ApplicationUser>>();
        var identityOptions = Options.Create(new IdentityOptions());
        var userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
        var pwdValidators = new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() };
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var logger = Substitute.For<ILogger<UserManager<ApplicationUser>>>();

        return new UserManager<ApplicationUser>(
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
    }

}