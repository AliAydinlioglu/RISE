using Microsoft.EntityFrameworkCore;
using Rise.Domain.Contact;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Contact;

namespace Rise.Services.Contact;

public class ContactService(ApplicationDbContext dbContext) : IContactService
{
    public async Task<Result<ContactResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var query = dbContext.Services.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(s => s.Name.Contains(request.SearchTerm) || 
                                    s.Description!.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(ctx);

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            query = request.OrderDescending
                ? query.OrderByDescending(e => EF.Property<object>(e, request.OrderBy))
                : query.OrderBy(e => EF.Property<object>(e, request.OrderBy));
        }
        else
        {
            query = query.OrderBy(s => s.ServiceCategory.Name).ThenBy(s => s.Name);
        }

        var services = await query
            .AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(s => ToIndexDto(s))
            .ToListAsync(ctx);

        return Result.Success(new ContactResponse.Index
        {
            Facilities = services,
            TotalCount = totalCount
        });
    }

    public async Task<Result<ContactResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default)
    {
        var service = await dbContext.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ctx);

        if (service == null)
        {
            Log.Warning("Service with ID {Id} not found.", id);
            return Result.NotFound($"Service with ID {id} not found.");
        }

        var detail = ToIndexDto(service);

        return Result.Success(new ContactResponse.Detail { Service = detail });
    }

    public async Task<Result<ContactResponse.Index>> GetByCategoryAsync(string categoryName, CancellationToken ctx = default)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            return Result.Error("Category name is required.");
        }

        var services = await dbContext.Services
            .AsNoTracking()
            .Where(s => s.ServiceCategory.Name == categoryName)
            .OrderBy(s => s.Name)
            .Select(s => ToIndexDto(s))
            .ToListAsync(ctx);

        return Result.Success(new ContactResponse.Index
        {
            Facilities = services,
            TotalCount = services.Count
        });
    }

    public async Task<Result<ContactResponse.Index>> GetByCampusAsync(string campusName, CancellationToken ctx = default)
    {
        if (string.IsNullOrWhiteSpace(campusName))
        {
            return Result.Error("Campus name is required.");
        }

        var services = await dbContext.Services
            .AsNoTracking()
            .Where(s => s.Location != null && s.Location.LocationName == campusName)
            .OrderBy(s => s.ServiceCategory.Name)
            .ThenBy(s => s.Name)
            .Select(s => ToIndexDto(s))
            .ToListAsync(ctx);

        return Result.Success(new ContactResponse.Index
        {
            Facilities = services,
            TotalCount = services.Count
        });
    }

    public async Task<Result<ContactResponse.Index>> GetStaticServicesAsync(CancellationToken ctx = default)
    {
        var services = await dbContext.Services
            .AsNoTracking()
            .Where(s => s.Location == null)
            .OrderBy(s => s.ServiceCategory.Name)
            .ThenBy(s => s.Name)
            .Select(s => ToIndexDto(s))
            .ToListAsync(ctx);

        return Result.Success(new ContactResponse.Index
        {
            Facilities = services,
            TotalCount = services.Count
        });
    }

    private static ContactDto.Index ToIndexDto(Facility service)
    {
        return new ContactDto.Index
        {
            Id = service.Id,
            Name = service.Name,
            ServiceCategoryName = service.ServiceCategory.Name,
            Description = service.Description,
            Location = service.Location != null ? new ContactDto.FacilityLocationDto
            {
                ServiceAddress = new ContactDto.StructuredAddressDto
                {
                    Street = service.Location.FaclitityAddress.Street,
                    HouseNumber = service.Location.FaclitityAddress.HouseNumber,
                    BusNumber = service.Location.FaclitityAddress.BusNumber,
                    Postcode = service.Location.FaclitityAddress.Postcode,
                    City = service.Location.FaclitityAddress.City
                },
                LocationName = service.Location.LocationName
            } : null,
            OpeningHours = service.OpeningHours.Select(oh => new ContactDto.ContactPeriodDto
            {
                ContactDate = oh.ContactDate,
                ContactHours = oh.ContactHours.Select(ch => new ContactDto.TimeRangeDto
                {
                    StartTime = ch.StartTime,
                    EndTime = ch.EndTime
                }).ToList()
            }).ToList(),
            Remarks = service.Remarks.ToList(),
            CommunicationChannels = service.CommunicationChannels.Select(cc => new ContactDto.CommunicationChannelDto
            {
                Name = cc.Name,
                Link = cc.Link,
                TypeOfCommunication = cc.TypeOfCommunication.ToString()
            }).ToList()
        };
    }
}

