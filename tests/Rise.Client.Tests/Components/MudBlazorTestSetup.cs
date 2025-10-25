using MudBlazor.Services;

namespace Rise.Client.Components;

public abstract class MudBlazorTestSetup : TestContext
{
    protected MudBlazorTestSetup()
    {
        Services.AddMudServices();

        // JSInterop-mock voor MudBlazor
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
    }
}
