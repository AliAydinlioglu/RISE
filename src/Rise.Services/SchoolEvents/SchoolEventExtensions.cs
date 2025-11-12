using Rise.Domain.Locations;
using Rise.Domain.SchoolEvents;
using Rise.Shared.Locations;
using Rise.Shared.SchoolEvents;

namespace Rise.Services.SchoolEvents;

public static class SchoolEventExtensions
{
    public static SchoolEventDto.Index ToIndexDto(this SchoolEvent se)
    {
        return new SchoolEventDto.Index
        {
            Id = se.Id,
            Title = se.Title,
            Description = se.Description,
            Date = se.Date,
            StartTime = se.TimeRange.StartTime,
            EndTime = se.TimeRange.EndTime,
            Price = se.Price,
            RegisterLink = se.RegisterLink,
            Capacity = se.Capacity,
            Registrable = se.Registrable,
            Publicity = se.Publicity,
            Category = se.Category,
            ImageUrl = se.ImageUrl,
            Location = se.Location.ToLocationDto(),
        };
    }

    public static SchoolEventDto.Detail ToDetailDto(this SchoolEvent se)
    {
        return new SchoolEventDto.Detail
        {
            Id = se.Id,
            Title = se.Title,
            Description = se.Description,
            Date = se.Date,
            StartTime = se.TimeRange.StartTime,
            EndTime = se.TimeRange.EndTime,
            Price = se.Price,
            RegisterLink = se.RegisterLink,
            Capacity = se.Capacity,
            Registrable = se.Registrable,
            Publicity = se.Publicity,
            Category = se.Category,
            ImageUrl = se.ImageUrl,
            Location = se.Location.ToLocationDto(),
        };
    }


    private static LocationDto.Index ToLocationDto(this Location loc)
    {
        return new LocationDto.Index
        {
            Id = loc.Id,
            Name = loc.Name,
            Street = loc.Street,
            HouseNumber = loc.HouseNumber,
            City = loc.City,
            Postcode = loc.Postcode,
            BusNumber = loc.BusNumber
        };
    }
}