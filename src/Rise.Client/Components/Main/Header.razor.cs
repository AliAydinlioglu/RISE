using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Main
{
    public partial class Header
    {
        
    void GoToLogin()
        {
            NavigationManager.NavigateTo("/login");
        }

        [Inject] NavigationManager NavigationManager { get; set; } = default!;
    }
}

