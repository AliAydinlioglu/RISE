using MudBlazor;
using MudBlazor.Services;

namespace Rise.Client.Components;

public abstract class MudBlazorTestSetup : TestContext
{
    protected MudBlazorTestSetup()
    {
        Services.AddMudServices();
        Services.AddMudServices(options =>
        {
            options.PopoverOptions.CheckForPopoverProvider = false;
        });

        // JSInterop-mock voor MudBlazor
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);

        JSInterop.Setup<int>("mudpopoverHelper.countProviders");

        JSInterop.Mode = JSRuntimeMode.Loose;
        
    }
    
}
