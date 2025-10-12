using Microsoft.Extensions.DependencyInjection;
using Rise.Persistence;
using Rise.Persistence.Queries.Calendar;
using Rise.Services.Calendar;
using Rise.Services.Products;
using Rise.Services.Projects;
using Rise.Services.User;
using Rise.Shared.Calendar;
using Rise.Services.StudentActivities;
using Rise.Shared.Products;
using Rise.Shared.Projects;
using Rise.Shared.User;
using Rise.Shared.StudentActivities;

namespace Rise.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // queries
        services.AddScoped<IGetCalendarQuery, GetCalendarQuery>();
        
        // Services
        services.AddScoped<IUserRepository, DummyUserService>();
        services.AddScoped<IProductService, ProductService>();        
        services.AddScoped<IProjectService, ProjectService>();        
        services.AddScoped<ICalendarService, CalendarService>();        
        services.AddTransient<DbSeeder>();       
        
        // Add other application services here.
        services.AddScoped<IStudentActivityService, StudentActivityService>();
        return services;
    }
}