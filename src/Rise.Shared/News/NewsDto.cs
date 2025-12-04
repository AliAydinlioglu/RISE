namespace Rise.Shared.News;

public static class NewsDto
{
    public abstract class Base
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Summary { get; set; }
        public IEnumerable<string>? ImageUrls { get; set; }
        public required DateTime PublishedAt { get; set; }
    }

    public class Index : Base
    {
    }

    public class Detail : Base
    {
        public required IEnumerable<string> ContentSections { get; set; }
    }
}

