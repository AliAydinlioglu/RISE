using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Layout;
using Rise.Client.Shared;
using System.Collections.Immutable;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Rise.Client.Pages;

public partial class Index
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
    [Inject] private IHomeBlockService HomeBlocks  {get ; set; } = null!;
    private IReadOnlySet<HomeBlockAttribute> AvailableBlocks = ImmutableHashSet<HomeBlockAttribute>.Empty;
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        
        AvailableBlocks = HomeBlocks.AvailableBlocks
            .Where(b => 
                b.Roles == null || 
                b.Roles.Any(r => authState.User.IsInRole(r)))
            .ToHashSet();
        
        // TODO: in other ticket, add sort and view logic based on user preferences and roles
    }
}