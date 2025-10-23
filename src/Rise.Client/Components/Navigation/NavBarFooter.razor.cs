using Microsoft.AspNetCore.Components;
using Rise.Shared.Navigation;

namespace Rise.Client.Components.Navigation;

public partial class NavBarFooter
{
    [Parameter] public HashSet<NavigationDto.Get> NavItems { get; set; } = new();
}
