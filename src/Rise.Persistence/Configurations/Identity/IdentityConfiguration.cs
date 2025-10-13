using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Identity;

namespace Rise.Persistence.Configurations.Identity;

/// <summary>
/// Configuration for the Identity tables.
/// </summary>
internal class IdentityConfiguration :
    IEntityTypeConfiguration<IdentityUser>,
    IEntityTypeConfiguration<Role>,
    IEntityTypeConfiguration<IdentityUserRole<string>>,
    IEntityTypeConfiguration<IdentityUserClaim<string>>,
    IEntityTypeConfiguration<IdentityUserLogin<string>>,
    IEntityTypeConfiguration<IdentityRoleClaim<string>>,
    IEntityTypeConfiguration<IdentityUserToken<string>>
{
    // Configures the IdentityUser tables.
    // NOTE:
    // If you want to use a separate schema for your Identity tables,
    // you can specify the schema as "auth" like this:
    //     builder.ToTable("Users", "auth");
    // However, be aware that SQLite does NOT support schemas, so this only works with SQL Server, MarioDB, PostgreSQL,...
    // The default below will work on any provider.
    
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
        => builder.ToTable("Users");

    public void Configure(EntityTypeBuilder<Role> builder)
        => builder.ToTable("Roles");

    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        => builder.ToTable("UserRoles");

    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        => builder.ToTable("UserClaims");

    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
        => builder.ToTable("UserLogins");

    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        => builder.ToTable("RoleClaims");

    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
        => builder.ToTable("UserTokens");
}