namespace Rise.Shared.Menu;

public static partial class WeekMenuRequest
{
    public class Resto
    {
        public int Id { get; set; }
    }
}

public static partial class WeekMenuResponse
{
    public class Resto
    {
        public RestoOverviewDto[] RestoOverview { get; set; } = []; 
    }   

}