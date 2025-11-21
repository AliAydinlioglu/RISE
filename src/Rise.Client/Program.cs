using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Rise.Client;
using Rise.Client.Calendar;
using Rise.Client.Courses;
using Rise.Client.Products;
using Rise.Client.SchoolEvents;
using Rise.Client.StudentActivities;
using Rise.Client.Shared;
using Rise.Shared;
using Rise.Shared.Calendar;
using Rise.Shared.Courses;
using Rise.Shared.Notifications;
using Rise.Shared.Products;
using Rise.Shared.SchoolEvents;
using Rise.Shared.StudentActivities;
using Rise.Shared.Contact;
using Rise.Client.Contact;
using DateTimeService = Rise.Client.DateTimeService;

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

    var baseUrl = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001");

    // MSAL Authentication (dit registreert automatisch AuthenticationStateProvider)
    builder.Services.AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        options.ProviderOptions.LoginMode = "redirect";
        options.ProviderOptions.Cache.CacheLocation = "localStorage";
        options.ProviderOptions.DefaultAccessTokenScopes.Add("api://8ae1a8dc-c2c6-44c9-bed8-7bbce3f84590/access_as_user");
    });

    builder.Services.AddHttpClient("SecureApi", client =>
    {
        client.BaseAddress = baseUrl;
    })
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
        .ConfigureHandler(
            authorizedUrls: [baseUrl.ToString()],
            scopes: ["api://8ae1a8dc-c2c6-44c9-bed8-7bbce3f84590/access_as_user"]
        ));

    // register the shared Singletons
    builder.Services.AddSingleton<IPageTitleService, PageTitleService>();
    builder.Services.AddSingleton<IHomeBlockService, HomeBlockService>();
    builder.Services.AddSingleton<IDateTimeService, DateTimeService>();

    builder.Services.AddHttpClient<IProductService, ProductService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICalendarService, CalendarService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICourseService, CourseService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IStudentActivityService, StudentActivityService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ISchoolEventService, SchoolEventService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
    
    builder.Services.AddHttpClient<IContactService, ContactService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
    
    builder.Services.AddHttpClient<INotificationService, NotificationService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
    
    builder.Services.AddMudServices();
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An exception occurred while creating the WASM host");
    throw;
}