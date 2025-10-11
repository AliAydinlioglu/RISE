using System.Text.Json.Serialization;

namespace Rise.Shared.StudentClubs;

public static class StudentClubDto
{
        public abstract class Base
        {
            public required int Id { get; set; }

            public required string Name { get; set; }
        }
        public class Index : Base
        {
           
            [JsonPropertyOrder(3)]
            public string? Description { get; set; }
            [JsonPropertyOrder(4)]
            public string? LogoUrl { get; set; }
        }

        public class Summary : Base
        {
        }
       

}