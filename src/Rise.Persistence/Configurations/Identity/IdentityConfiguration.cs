using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Persistence.Models.Identity;

namespace Rise.Persistence.Configurations.Identity;

/// <summary>
/// Configuration for the Identity tables.
/// </summary>
internal class IdentityConfiguration :
    IEntityTypeConfiguration<ApplicationUser>,
    IEntityTypeConfiguration<ApplicationRole>,
    IEntityTypeConfiguration<IdentityUserRole<Guid>>,
    IEntityTypeConfiguration<IdentityUserClaim<Guid>>,
    IEntityTypeConfiguration<IdentityUserLogin<Guid>>,
    IEntityTypeConfiguration<IdentityRoleClaim<Guid>>,
    IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    // Configures the IdentityUser tables.
    // NOTE:
    // If you want to use a separate schema for your Identity tables,
    // you can specify the schema as "auth" like this:
    //     builder.ToTable("Users", "auth");
    // However, be aware that SQLite does NOT support schemas, so this only works with SQL Server, MarioDB, PostgreSQL,...
    // The default below will work on any provider.

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
        builder.Property(x => x.FirstName).HasMaxLength(200);
        builder.Property(x => x.LastName).HasMaxLength(200);
        builder.Property(x => x.ClassGroup).HasMaxLength(50);
        builder.Property(x => x.SsoId).IsRequired();
        builder.Property(x => x.SsoProvider).IsRequired().HasMaxLength(255);
        builder.Property(x => x.SsoRefreshToken).IsRequired();
    }

    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        => builder.ToTable("Roles");

    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        => builder.ToTable("UserRoles");

    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        => builder.ToTable("UserClaims");

    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
        => builder.ToTable("UserLogins");

    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        => builder.ToTable("RoleClaims");

    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        => builder.ToTable("UserTokens");
}