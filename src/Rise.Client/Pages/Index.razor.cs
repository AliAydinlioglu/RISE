using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Layout;
using Rise.Client.Shared;
using System.Collections.Immutable;
using System.Reflection;
using System.Security.Claims;

namespace Rise.Client.Pages;

public partial class Index
{
    
    [Inject] private IHomeBlockService HomeBlocks  {get ; set; } = null!;
    private IReadOnlySet<HomeBlockAttribute> AvailableBlocks = ImmutableHashSet<HomeBlockAttribute>.Empty;
    protected override void OnInitialized()
    {
        AvailableBlocks = HomeBlocks.AvailableBlocks;
        
        // TODO: in other ticket, add sort and view logic based on user preferences and roles
    }
}