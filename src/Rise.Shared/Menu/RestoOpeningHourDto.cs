namespace Rise.Shared.Menu;

public class RestoOpeningHourDto
{
    public DateOnly Date { get; set; }
    public (TimeOnly From, TimeOnly To)[] Hours { get; set; } = [];

}