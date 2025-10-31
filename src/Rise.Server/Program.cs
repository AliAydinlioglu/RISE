using Destructurama;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Rise.Persistence;
using Rise.Persistence.Models.Identity;
using Rise.Persistence.Triggers;
using Rise.Server.Identity;
using Rise.Server.Processors;
using Rise.Services;
using Rise.Services.Identity;
using Serilog.Events;
using Pomelo.EntityFrameworkCore.MySql;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // Initial log setup, will be overwritten by Serilog, but we need a logger before Dependency Injection is activated.

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    
    builder.Services
        .AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration) // Configuration in AppSettings.json
            .Destructure.UsingAttributes()) // Sensitive data logging
        .AddIdentity<ApplicationUser, ApplicationRole>() 
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .Services.AddDbContext<ApplicationDbContext>(o =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ??
                                   throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            o.UseMySql(connectionString, serverVersion);
            o.EnableDetailedErrors();
            if (builder.Environment.IsDevelopment())
            {
                o.EnableSensitiveDataLogging(); // only enabled in development.
            }
            o.UseTriggers(options => options.AddTrigger<EntityBeforeSaveTrigger>()); // Handles all UpdatedAt, CreatedAt stuff.
        })
        .AddHttpContextAccessor()
        .AddScoped<ISessionContextProvider, HttpContextSessionProvider>() // Provides the current user from the HttpContext to the session provider.
        .AddApplicationServices() // You'll need to add your own services in this function call.
        .AddAuthorization()
        .AddFastEndpoints(o =>
        {
            o.IncludeAbstractValidators = true; // Include validators from abstract classes (see https://docs.fluentvalidation.net/en/latest/).
            o.Assemblies = [typeof(Rise.Shared.Products.ProductRequest).Assembly,typeof(Rise.Shared.StudentActivities.StudentActivityRequest).Assembly ]; // Adds the validators from other assemblies
        })
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "RISE API";
            };
        })
#if DEBUG
        .AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", policy => policy
                .WithOrigins("https://localhost:5001")
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
#else
        .AddCors(options =>
        {
            options.AddPolicy("FrontendPolicy", policy =>
            {
                var frontendUrl = builder.Configuration["FrontendUrl"]; //TODO: add FrontendUrl
                policy.WithOrigins(frontendUrl)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
#endif

    var app = builder.Build();
    // apply Database migraticons on startup, not so wise in production (Use Generated SQL Scripts) 
    // See: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
    if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Test")
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
            // In Dev/Test: ensure schema exists even if migrations are incomplete
            dbContext.Database.EnsureCreated();
            try
            {
                dbContext.Database.Migrate(); // Apply migrations if present
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Migrate failed; proceeding with EnsureCreated schema in {Env}.", app.Environment.EnvironmentName);
            }
            var canSeed = false;
            try
            {
                // If Identity tables exist, this query will succeed
                _ = await dbContext.Users.AnyAsync();
                canSeed = true;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Base tables not present; attempting full schema (re)create in {Env}.", app.Environment.EnvironmentName);
                try
                {
                    await dbContext.Database.EnsureDeletedAsync();
                    await dbContext.Database.EnsureCreatedAsync();
                    canSeed = true;
                }
                catch (Exception ex2)
                {
                    Log.Warning(ex2, "Failed to (re)create schema in {Env}.", app.Environment.EnvironmentName);
                }
            }
            if (canSeed)
            {
                await dbSeeder.SeedAsync();
            }
        }
    }
    // Theses middlewares are strict in order of calling!
    app.UseHttpsRedirection()
        .UseBlazorFrameworkFiles() // Blazor is also served from the API. 
        .UseStaticFiles()
        .UseDefaultExceptionHandler()
        .UseAuthentication()
        .UseAuthorization()
        .UseFastEndpoints(o =>
        {
            o.Endpoints.Configurator = ep =>
            {
                ep.DontAutoSendResponse();
                ep.PreProcessor<GlobalRequestLogger>(Order.Before);
                ep.PostProcessor<GlobalResponseSender>(Order.Before);
                ep.PostProcessor<GlobalResponseLogger>(Order.Before);
                
            };
        });
#if DEBUG
        .UseCors("AllowLocalhost");
#else
        app.UseCors("FrontendPolicy");
#endif
        
    app.MapFallbackToFile("index.html"); // Serves the Blazor app from the API, when no routes match.
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}


