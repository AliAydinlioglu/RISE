using System.Collections.Concurrent;

namespace Rise.Client.Services;

public interface IPaginationStateService
{
    void SetPage(string key, int page);
    int? GetPage(string key);
}

public class PaginationStateService : IPaginationStateService
{
    private readonly Dictionary<string, int> _pages = new();

    public void SetPage(string key, int page)
    {
        _pages[key] = page;
    }

    public int? GetPage(string key)
    {
        return _pages.TryGetValue(key, out var page) ? page : null;
    }


}
