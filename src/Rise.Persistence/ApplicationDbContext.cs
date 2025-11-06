using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Calendar;
using Rise.Domain.Contact;
using Rise.Domain.Identity;
using Rise.Domain.Locations;
using Rise.Domain.Navigation;
using Rise.Domain.Products;
using Rise.Domain.Projects;
using Rise.Domain.SchoolEvents;
using Rise.Domain.StudentActivities;
using Rise.Persistence.Models.Identity;

namespace Rise.Persistence;

/// <summary>
/// Entrance to the database, inherits from IdentityDbContext and is basically a Unit Of Work and Repository pattern combined.
/// A <see cref="DbSet"/> is a repository for a specific type of entity.
/// The <see cref="ApplicationDbContext"/> is the Unit Of Work pattern
/// Will look very similar when switching database providers.
/// See https://hogent-web.github.io/csharp/chapters/09/slides/index.html#1
/// See https://enterprisecraftsmanship.com/posts/should-you-abstract-database/
/// </summary>
/// <param name="opts"></param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(opts)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<RoleNavigationItemContentLocation> RoleNavigationItems => Set<RoleNavigationItemContentLocation>();
    public DbSet<NavigationItem> NavigationItems => Set<NavigationItem>();
    public DbSet<ContentLocation> ContentLocations => Set<ContentLocation>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<StudentActivity> StudentActivities => Set<StudentActivity>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<StudentClub> StudentClubs => Set<StudentClub>();
    public DbSet<SchoolEvent> SchoolEvents => Set<SchoolEvent>();
    public DbSet<Facility> Services => Set<Facility>();
    
    public DbSet<AcademicSemester> AcademicSemesters => Set<AcademicSemester>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Deadline> Deadlines => Set<Deadline>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<Role> DomainRoles => Set<Role>();
  
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // All columns in the mariadb have a maxlength of 255 for string values.
        // there is a maximum length that can be indexed by a database.
        // Some columns need more length, but these can be set on the configuration level for that Entity in particular.
        configurationBuilder.Properties<string>().HaveMaxLength(255);
        // All decimals columns should have 2 digits after the comma
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Applying all types of IEntityTypeConfiguration in the Persistence project.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
