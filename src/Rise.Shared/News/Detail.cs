namespace Rise.Shared.News;

public static partial class NewsRequest
{
    public class Detail
    {
        public int Id { get; set; }
    }
}

public static partial class NewsResponse
{
    public class Detail
    {
        public NewsDto.Detail NewsItem { get; set; }
    }
}

