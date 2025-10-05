namespace Rise.Shared.StudentClubs;

public static class StudentClubDto
{
        public class Index
        {
            public required int Id { get; set; }

            public required string Name { get; set; }
            public string? Description { get; set; }
            public string? LogoUrl { get; set; }
        }

        public class Summary
        {
            public required int Id { get; set; }  
            public required string Name { get; set; }
        }
       

}