namespace Rise.Client.Components.CardComponent;

public static class CardTestData
{
    public static TheoryData<string, string?, string?> CourseInfoData => new()
    {
        { "RISE", "C. De Leenheer", "14:00 | GSCHB.1.001" },
        { "RISE", null, "14:00 | GSCHB.1.001" },
        { "RISE", "C. De Leenheer", null },
        { "RISE", null, null }
    };

    public static TheoryData<DateTime, string, string> DatesData => new()
    {
        { new DateTime(2025, 1, 2), "02", "jan" },
        { new DateTime(2025, 2, 4), "04", "feb" },
        { new DateTime(2025, 3, 6), "06", "mrt" },
        { new DateTime(2025, 4, 8), "08", "apr" },
        { new DateTime(2025, 5, 10), "10", "mei" },
        { new DateTime(2025, 6, 12), "12", "jun" },
        { new DateTime(2025, 7, 14), "14", "jul" },
        { new DateTime(2025, 8, 16), "16", "aug" },
        { new DateTime(2025, 9, 18), "18", "sep" },
        { new DateTime(2025, 10, 20), "20", "okt" },
        { new DateTime(2025, 11, 25), "25", "nov" },
        { new DateTime(2025, 12, 30), "30", "dec" }
    };

    public static TheoryData<string, string> BackgroundTitleData => new()
    {
        { "RISE", "RISE" },
        { "RI SE", "RISE" },
        { "RiSe", "RS" },
        { "rISe", "IS" },
        { "rise", "" },
        { "r i s e", "" },
        { "IT Fundamentals", "ITF" }
    };
}
