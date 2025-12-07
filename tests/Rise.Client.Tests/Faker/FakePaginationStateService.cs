using Rise.Client.Services;

namespace Rise.Client.Faker;

public class FakePaginationStateService : IPaginationStateService
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

