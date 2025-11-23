using Rise.Shared.Locations;

namespace Rise.Shared.Menu;

public class RestoOverviewDto
{
    public string Name { get; set; } = null!;
    public LocationDto.Index Location { get; set; } = null!;
    public RestoOpeningHourDto[] OpeningHours { get; set; } = [];
    public bool IsFavorite { get; set; }    
}