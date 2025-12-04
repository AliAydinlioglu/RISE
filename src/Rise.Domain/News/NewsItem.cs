namespace Rise.Domain.News;

public class NewsItem : Entity
{
    private readonly List<string> _contentSections = new();
    private readonly List<string> _imageUrls = new();

    public string Title { get; private set; } = default!;
    public string Summary { get; private set; } = default!;
    public IReadOnlyCollection<string> ContentSections => _contentSections.AsReadOnly();
    public IReadOnlyCollection<string> ImageUrls => _imageUrls.AsReadOnly();
    public DateTime PublishedAt { get; private set; }

    public NewsItem(string title, string summary, List<string> contentSections, List<string>? imageUrls, DateTime publishedAt)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
        Summary = Guard.Against.NullOrWhiteSpace(summary);
        Guard.Against.NullOrEmpty(contentSections);
        
        foreach (var content in contentSections)
        {
            _contentSections.Add(Guard.Against.NullOrWhiteSpace(content));
        }
        
        if (imageUrls != null)
        {
            foreach (var imageUrl in imageUrls)
            {
                _imageUrls.Add(Guard.Against.NullOrWhiteSpace(imageUrl));
            }
        }
        
        PublishedAt = Guard.Against.NullOrOutOfSQLDateRange(publishedAt);
    }
}

