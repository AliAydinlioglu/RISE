using Microsoft.Extensions.DependencyInjection;
using Rise.Persistence;
using Rise.Persistence.Queries.Calendar;
using Rise.Services.Calendar;
using Rise.Services.Navigation;
using Rise.Services.Products;
using Rise.Services.Projects;
using Rise.Services.User;
using Rise.Shared;
using Rise.Shared.Calendar;
using Rise.Services.StudentActivities;
using Rise.Shared.Navigation;
using Rise.Shared.Products;
using Rise.Shared.Projects;
using Rise.Shared.User;
using Rise.Shared.StudentActivities;
using Rise.Services.Identity;
using Rise.Shared.Identity;

namespace Rise.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IUserRepository, DummyUserService>();
        services.AddScoped<IProductService, ProductService>();        
        services.AddScoped<IProjectService, ProjectService>();        
        services.AddScoped<ICalendarService, CalendarService>();        
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICalendarService, CalendarService>();   
        services.AddScoped<IDateTimeService, DateTimeService>();   
        services.AddTransient<DbSeeder>();
        
        // queries
        services.AddScoped<IGetCalendarQuery, GetCalendarQuery>();    
        
        // Add other application services here.
        services.AddScoped<IStudentActivityService, StudentActivityService>();
        return services;
    }
}