using Rise.Domain.News;

namespace Rise.TestDoubles;

public static class NewsTestDataFactory
{
    public static NewsItem CreateDefaultNewsItem()
    {
        return new NewsItem(
            "Test Nieuws Titel",
            "Dit is een test samenvatting van het nieuwsitem.",
            new List<string>
            {
                "Dit is de eerste paragraaf van de inhoud.",
                "Dit is de tweede paragraaf met meer informatie."
            },
            new List<string> { "/images/news1.jpg" },
            DateTime.Now.AddDays(-1)
        );
    }

    public static NewsItem CreateNewsItem(
        string title,
        string summary,
        List<string> contentSections,
        List<string>? imageUrls,
        DateTime publishedAt)
    {
        return new NewsItem(title, summary, contentSections, imageUrls, publishedAt);
    }

    public static List<NewsItem> CreateTestNewsItems(int count)
    {
        var newsItems = new List<NewsItem>();

        for (int i = 1; i <= count; i++)
        {
            var publishedDate = DateTime.Now.AddDays(-i);
            var newsItem = new NewsItem(
                $"Nieuws Titel {i}",
                $"Samenvatting van nieuws item {i}",
                new List<string>
                {
                    $"Content sectie 1 van nieuws {i}",
                    $"Content sectie 2 van nieuws {i}"
                },
                new List<string> { $"/images/news{i}.jpg" },
                publishedDate
            );

            newsItems.Add(newsItem);
        }

        return newsItems;
    }

    public static NewsItem CreateNewsItemWithMultipleImages(int imageCount)
    {
        var imageUrls = new List<string>();
        for (int i = 1; i <= imageCount; i++)
        {
            imageUrls.Add($"/images/news-image-{i}.jpg");
        }

        return new NewsItem(
            "Nieuws met meerdere afbeeldingen",
            "Dit nieuwsitem heeft meerdere afbeeldingen.",
            new List<string>
            {
                "Eerste paragraaf van de inhoud.",
                "Tweede paragraaf van de inhoud.",
                "Derde paragraaf van de inhoud.",
                "Vierde paragraaf van de inhoud."
            },
            imageUrls,
            DateTime.Now.AddDays(-1)
        );
    }

    public static NewsItem CreateNewsItemWithNoImages()
    {
        return new NewsItem(
            "Nieuws zonder afbeeldingen",
            "Dit nieuwsitem heeft geen afbeeldingen.",
            new List<string>
            {
                "Eerste paragraaf van de inhoud.",
                "Tweede paragraaf van de inhoud."
            },
            null,
            DateTime.Now.AddDays(-2)
        );
    }
}

