using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Rise.Client;
using Rise.Client.Calendar;
using Rise.Client.Contact;
using Rise.Client.Courses;
using Rise.Client.Identity;
using Rise.Client.Offline;
using Rise.Client.Identity;
using Rise.Client.Offline;
using Rise.Client.Products;
using Rise.Client.Restaurant;
using Rise.Client.Restaurant.Components;
using Rise.Client.SchoolEvents;
using Rise.Client.Services;
using Rise.Client.Shared;
using Rise.Client.StudentActivities;
using Rise.Client.Theme;
using Rise.Client.UserPreferences;
using Rise.Client.UserPreferences.Services;
using Rise.Shared;
using Rise.Shared.Calendar;
using Rise.Shared.Contact;
using Rise.Shared.Courses;
using Rise.Shared.Menu;
using Rise.Shared.Notifications;
using Rise.Shared.Products;
using Rise.Shared.SchoolEvents;
using Rise.Shared.StudentActivities;
using Rise.Shared.Contact;
using Rise.Client.Contact;
using Rise.Client.Theme;
using Rise.Client.Restaurant;
using Rise.Shared.UserPreferences;
using TG.Blazor.IndexedDB;
using Rise.Shared.Menu;
using Rise.Client.Services;
using Rise.Client.StudentActivities;
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
        options.ProviderOptions.DefaultAccessTokenScopes.Add(
            "api://8ae1a8dc-c2c6-44c9-bed8-7bbce3f84590/access_as_user");
    });

    builder.Services.AddIndexedDB(dbStore =>
    {
        dbStore.DbName = "RiseCampusApp";
        dbStore.Version = 1;
        dbStore.Stores.Add(new StoreSchema
            { Name = "RiseOfflineCache", PrimaryKey = new IndexSpec { KeyPath = "key" } });
    });

    builder.Services.AddHttpClient("SecureApi", client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: [baseUrl.ToString()],
                scopes: ["api://8ae1a8dc-c2c6-44c9-bed8-7bbce3f84590/access_as_user"]
            ));

    // register the shared Singletons
    builder.Services.AddSingleton<IPageTitleService, PageTitleService>();
    builder.Services.AddSingleton<IHomeBlockService, HomeBlockService>();
    builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
    builder.Services.AddSingleton<IEventStreamService, EventStreamService>();
    builder.Services.AddSingleton<IThemingService, ThemingService>();
    builder.Services.AddScoped<IUserPreferenceStateService, UserPreferenceStateService>();
    builder.Services.AddScoped<IPaginationStateService, PaginationStateService>();
    builder.Services.AddScoped<IIndexedDbManager, RiseIndexedDbManager>();
    builder.Services.AddScoped<ICacheService, CacheService>();
    builder.Services.AddScoped<RiseHttpMessageHandler>();
    builder.Services.AddScoped<IConnectionService, ConnectionService>();

    builder.Services.AddScoped<IRestaurantSelectionService, RestaurantSelectionStateService>();
    builder.Services.AddScoped<IFavouriteRestoService, FavouriteRestoService>();

    builder.Services.AddHttpClient<IPriceListService, PriceListService>(client =>
    {
        client.BaseAddress = baseUrl;
    });

    builder.Services.AddHttpClient<IRestoService, RestoService>(client =>
    {
        client.BaseAddress = baseUrl;
    });
    
    builder.Services.AddHttpClient<IProductService, ProductService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IMenuService, MenuService>(client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler<RiseHttpMessageHandler>();

    builder.Services.AddHttpClient<IPriceListService, PriceListService>(client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler<RiseHttpMessageHandler>();

    builder.Services.AddHttpClient<IRestoService, RestoService>(client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler<RiseHttpMessageHandler>();

    builder.Services.AddHttpClient<IProductService, ProductService>(client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler<RiseHttpMessageHandler>()
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICalendarService, CalendarService>(client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler<RiseHttpMessageHandler>()
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICourseService, CourseService>(client => { client.BaseAddress = baseUrl; })
        .AddHttpMessageHandler<RiseHttpMessageHandler>()
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IStudentActivityService, StudentActivityService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<RiseHttpMessageHandler>();

    builder.Services.AddHttpClient<ISchoolEventService, SchoolEventService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
    
    builder.Services.AddHttpClient<IContactService, ContactService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IUserPreferenceService, UserPreferenceService>(client =>
    {
        client.BaseAddress = baseUrl;
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddSingleton<IAppRoleStateService, AppRoleStateService>();

    builder.Services.AddMudServices();
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An exception occurred while creating the WASM host");
    throw;
}