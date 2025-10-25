using MudBlazor.Services;

namespace Rise.Client.Components;

public abstract class MudBlazorTestBase : TestContext
{
    protected MudBlazorTestBase()
    {
        Services.AddMudServices();

        // JSInterop-mock voor MudBlazor
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
    }
}
