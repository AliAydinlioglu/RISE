using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Theme;
using Rise.Shared.Common;
using Rise.Shared.News;

namespace Rise.Client.News;

[HomeBlock(icon: @Icons.Material.Outlined.Newspaper, label: "Nieuws", route: "/news")]
public partial class Index
{
    private IEnumerable<NewsDto.Index>? newsItems;
    private bool _isLoading;
    private bool _isError;
    [Inject] public required INewsService NewsService { get; set; }
    [Inject] private IThemingService ThemingService { get; set; } = null!;
    private int currentPage = 1;
    private int pageSize = 8;
    private int totalCount = 0;
    private int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);

    protected override async Task OnInitializedAsync()
    {
        await LoadNewsItemsAsync();
    }

    private async Task LoadNewsItemsAsync()
    {
        try
        {
            _isLoading = true;
            _isError = false;
            var request = new QueryRequest.SkipTake
            {
                Skip = (currentPage - 1) * pageSize,
                Take = pageSize,
            };

            var result = await NewsService.GetIndexAsync(request, CancellationToken.None);
            newsItems = result.Value.NewsItems;
            totalCount = result.Value.TotalCount;
        }
        catch
        {
            _isError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task OnPageChangedAsync(int page)
    {
        currentPage = page;
        await LoadNewsItemsAsync();
    }

    private string GetBackgroundImageStyle(string? imageUrl)
    {
        if (ThemingService.ImagesOff || string.IsNullOrEmpty(imageUrl))
            return "";
        
        return $"background-image: url('{imageUrl}');";
    }
}

