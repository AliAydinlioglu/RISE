using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Rise.Client;
using Rise.Client.Calendar;
using Rise.Client.Identity;
using Rise.Client.Products;
using Rise.Client.Shared;
using Rise.Shared;
using Rise.Shared.Calendar;
using Rise.Client.StudentActivities;
using Rise.Shared.Products;
using Rise.Shared.StudentActivities;

try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.BrowserConsole(outputTemplate: "[{Timestamp:HH:mm:ss}{Level:u3}]{Message:lj} {NewLine}{Exception}")
        .CreateLogger();

    Log.Information("Starting web application");

// register the cookie handler
    builder.Services.AddTransient<CookieHandler>();

// set up authorization
    builder.Services.AddAuthorizationCore();

// register the custom state provider
    builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
// register the shared title service
    builder.Services.AddSingleton<SharedPageTitleService>();

// register the account management interface
    builder.Services.AddScoped(sp => (IAccountManager)sp.GetRequiredService<AuthenticationStateProvider>());

// configure client for auth interactions
    var baseUrl = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001");
    builder.Services.AddHttpClient("SecureApi", opt => opt.BaseAddress = baseUrl)
        .AddHttpMessageHandler<CookieHandler>();

    builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
    builder.Services.AddHttpClient<IProductService, ProductService>(client => { client.BaseAddress = baseUrl; });
    builder.Services.AddHttpClient<ICalendarService, CalendarService>(client => { client.BaseAddress = baseUrl; });
    builder.Services.AddHttpClient<IStudentActivityService, StudentActivityService>(client => { client.BaseAddress = baseUrl; }); 
    builder.Services.AddMudServices();
    
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An exception occurred while creating the WASM host");
    throw;
}