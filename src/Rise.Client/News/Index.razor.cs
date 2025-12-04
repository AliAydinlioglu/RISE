using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Common;
using Rise.Shared.News;

namespace Rise.Client.News;

[HomeBlock(icon: @Icons.Material.Outlined.Newspaper, label: "Nieuws", route: "/news")]
public partial class Index
{
    private IEnumerable<NewsDto.Index>? newsItems;
    [Inject] public required INewsService NewsService { get; set; }
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
        var request = new QueryRequest.SkipTake
        {
            Skip = (currentPage - 1) * pageSize,
            Take = pageSize,
        };

        var result = await NewsService.GetIndexAsync(request, CancellationToken.None);
        newsItems = result.Value.NewsItems;
        totalCount = result.Value.TotalCount;
    }

    private async Task OnPageChangedAsync(int page)
    {
        currentPage = page;
        await LoadNewsItemsAsync();
    }
}

