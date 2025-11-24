namespace Rise.Shared.Menu;

public static partial class MenuRequest
{
    public class Resto
    {
        public int Id { get; set; }
    }
}

public static partial class MenuResponse
{
    public class RestoOverview
    {
        public RestoOverviewDto[] Restos { get; set; } = []; 
    }   
    
    public class RestoDetail
    {
        public RestoDetailDto Resto { get; set; } = null!;
    }  

}