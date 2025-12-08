namespace Rise.Shared.News;

public static partial class NewsResponse
{
    public class Index
    {
        public IEnumerable<NewsDto.Index> NewsItems { get; set; } = [];
        public int TotalCount { get; set; }
    }
}

