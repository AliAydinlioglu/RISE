using System.Reflection;
using Rise.Client.Attributes;

namespace Rise.Client.Shared;

public interface IHomeBlockService
{
    IReadOnlySet<HomeBlockAttribute> AvailableBlocks { get; }
}

public class HomeBlockService : IHomeBlockService
{
    public IReadOnlySet<HomeBlockAttribute> AvailableBlocks { get; } =
        Assembly.GetExecutingAssembly()
            .GetTypes()
            .Select(t => t.GetCustomAttribute<HomeBlockAttribute>())
            .OfType<HomeBlockAttribute>()
            .ToHashSet();
}