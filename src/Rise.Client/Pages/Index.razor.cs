using System.Collections.Immutable;
using System.Reflection;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Layout;

namespace Rise.Client.Pages;

public partial class Index
{
    private HashSet<HomeBlockAttribute> HomeBlocks  = [];
    protected override void OnInitialized()
    {
        // Get all types with [HomeBlock] in the current assembly
        var assembly = Assembly.GetExecutingAssembly();

        HomeBlocks = assembly.GetTypes()
            .Select(t => t.GetCustomAttribute<HomeBlockAttribute>())
            .Where(attr => attr is not null)
            .ToHashSet()!;
        
        // TODO: in other ticket, add sort and view logic based on user preferences and roles
    }
}