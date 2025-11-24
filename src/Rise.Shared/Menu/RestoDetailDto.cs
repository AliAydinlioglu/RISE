using Rise.Shared.Locations;

namespace Rise.Shared.Menu;

public class RestoDetailDto
{
    public string Name { get; set; } = null!;
    public RestoOpeningHourDto[] OpeningHours { get; set; } = [];
    public LocationDto.Index Location { get; set; } = null!;
}