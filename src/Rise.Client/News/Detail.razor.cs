using Microsoft.AspNetCore.Components;
using Rise.Shared.News;

namespace Rise.Client.News;

public partial class Detail
{
    private NewsDto.Detail? newsItem;
    [Inject] public required INewsService NewsService { get; set; }

    [Parameter] public string? Id { get; set; }

    private string Title { get; set; } = string.Empty;
    private string Summary { get; set; } = string.Empty;
    private IEnumerable<string> ContentSections { get; set; } = Enumerable.Empty<string>();
    private IEnumerable<string>? ImageUrls { get; set; }
    private List<ColumnItem> LeftColumnItems { get; set; } = new();
    private List<ColumnItem> RightColumnItems { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return;
        }

        var idValue = int.Parse(Id);
        var result = await NewsService.GetDetailByIdAsync(idValue, CancellationToken.None);
        newsItem = result.Value.NewsItem;
        
        if (newsItem != null)
        {
            Title = newsItem.Title;
            Summary = newsItem.Summary;
            ContentSections = newsItem.ContentSections;
            ImageUrls = newsItem.ImageUrls;
            
            DistributeContentAndImages();
        }
    }

    private void DistributeContentAndImages()
    {
        LeftColumnItems.Clear();
        RightColumnItems.Clear();

        var contentList = ContentSections.ToList();
        var imageList = ImageUrls?.ToList() ?? new List<string>();

        if (imageList.Count == 1)
        {
            int contentMidpoint = (contentList.Count + 1) / 2;
            
            for (int i = 0; i < contentMidpoint; i++)
            {
                LeftColumnItems.Add(new ColumnItem 
                { 
                    IsContent = true, 
                    Content = contentList[i] 
                });
            }

            RightColumnItems.Add(new ColumnItem 
            { 
                IsContent = false, 
                ImageUrl = imageList[0] 
            });
            
            for (int i = contentMidpoint; i < contentList.Count; i++)
            {
                RightColumnItems.Add(new ColumnItem 
                { 
                    IsContent = true, 
                    Content = contentList[i] 
                });
            }
        }
        else
        {
            int contentMidpoint = (contentList.Count + 1) / 2;
            int imageMidpoint = (imageList.Count + 1) / 2;

            for (int i = 0; i < contentMidpoint; i++)
            {
                LeftColumnItems.Add(new ColumnItem 
                { 
                    IsContent = true, 
                    Content = contentList[i] 
                });
                
                if (i < imageMidpoint)
                {
                    LeftColumnItems.Add(new ColumnItem 
                    { 
                        IsContent = false, 
                        ImageUrl = imageList[i] 
                    });
                }
            }

            for (int i = contentMidpoint; i < contentList.Count; i++)
            {
                RightColumnItems.Add(new ColumnItem 
                { 
                    IsContent = true, 
                    Content = contentList[i] 
                });
                
                int imageIndex = i - contentMidpoint + imageMidpoint;
                if (imageIndex < imageList.Count)
                {
                    RightColumnItems.Add(new ColumnItem 
                    { 
                        IsContent = false, 
                        ImageUrl = imageList[imageIndex] 
                    });
                }
            }
        }
    }

    private class ColumnItem
    {
        public bool IsContent { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
