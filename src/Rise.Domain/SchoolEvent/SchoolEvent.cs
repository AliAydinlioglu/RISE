using Rise.Domain.Locations;
using Rise.Domain.StudentActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.SchoolEvents
{
    public class SchoolEvent : Entity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public TimeRange TimeRange { get; private set; }
        public decimal Price { get; private set; }
        public string RegisterLink { get; private set; }
        public int Capacity { get; private set; }
        public bool Registrable { get; private set; }
        public string Publicity { get; private set; }
        public string? ImageUrl { get; private set; }
        public Location Location { get; set; }

        private SchoolEvent() { }
        public SchoolEvent(string title, string description, DateTimeOffset date, TimeRange timeRange, decimal price, string registerLink, int capacity, bool registrable, string publicity,
            string imageUrl, Location location)
        {
            Title = Guard.Against.NullOrWhiteSpace(title);
            Description = description;
            Date = Guard.Against.NullOrOutOfSQLDateRange(date.UtcDateTime);
            TimeRange = Guard.Against.Null(timeRange);
            ImageUrl = imageUrl;
            Location = Guard.Against.Null(location);
            Price = Guard.Against.Null(price);
            RegisterLink = Guard.Against.NullOrWhiteSpace(registerLink);
            Registrable = Guard.Against.Null(registrable);
            Capacity = Guard.Against.NegativeOrZero(capacity);
            Publicity = Guard.Against.NullOrWhiteSpace(publicity);
        }
    }
}
