using Rise.Domain.Locations;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Locations;
using Rise.Shared.StudentClubs;
using Rise.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rise.Shared.SchoolEvents;
using Rise.Domain.SchoolEvents;
using Microsoft.EntityFrameworkCore;

namespace Rise.Services.SchoolEvents
{
    public class SchoolEventService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : ISchoolEventService
    {
        public async Task<Result<SchoolEventResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
        {
            var query = dbContext.SchoolEvents
                .Include(sa => sa.Location)
                .AsQueryable();

            var totalCount = await query.CountAsync(ctx);

            var studentEvents = await query.AsNoTracking()
                .Skip(request.Skip)
                .Take(request.Take)
                .Select(sa => ToIndexDto(sa))
                .ToListAsync(ctx);

            return Result.Success(new SchoolEventResponse.Index
            {
                SchoolEvents = studentEvents,
                TotalCount = totalCount,
            });
        }

        public async Task<Result<SchoolEventResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
        {
            var studentEvent = await dbContext.SchoolEvents
                .Include(sa => sa.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(sa => sa.Id == id, ctx);

            if (studentEvent == null)
                return Result.NotFound($"Student event with ID {id} not found.");

            var detail = ToDetailDto(studentEvent);

            return Result.Success(new SchoolEventResponse.Detail { SchoolEvent = detail });
        }

        public static SchoolEventDto.Index ToIndexDto(SchoolEvent se)
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
                Location = ToLocationDto(se.Location),
            };
        }

        public static SchoolEventDto.Detail ToDetailDto(SchoolEvent se)
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
                Location = ToLocationDto(se.Location),
            };
        }


        public static LocationDto.Index ToLocationDto(Location loc)
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
}