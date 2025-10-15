using Microsoft.Extensions.DependencyInjection;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Services.Navigation;
using Rise.Services.Products;
using Rise.Services.Projects;
using Rise.Shared.Identity;
using Rise.Shared.Navigation;
using Rise.Shared.Products;
using Rise.Shared.Projects;

namespace Rise.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();        
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<DbSeeder>();
        
        // Add other application services here.
        return services;
    }
}