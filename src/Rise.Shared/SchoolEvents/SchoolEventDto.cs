using Rise.Shared.Locations;
using Rise.Shared.StudentClubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rise.Shared.SchoolEvents
{
    public static class SchoolEventDto
    {
        public abstract class Base
        {
            public required int Id { get; set; }
            public required string Title { get; set; }
            public string? Description { get; set; }
            public required DateTimeOffset Date { get; set; }
            public required TimeOnly StartTime { get; set; }
            public required TimeOnly EndTime { get; set; }
            public required decimal Price { get; set; }
            public required string RegisterLink { get; set; }
            public required int Capacity { get; set; }
            public required bool Registrable { get; set; }
            public required string Publicity { get; set; }
            public required string Category { get; set; }
            public required string? ImageUrl { get; set; }
            public required LocationDto.Index Location { get; set; }
        }
        public class Index : Base { }
        public class Detail : Base { }
    }
}
