namespace Rise.Shared.Projects;

public static partial class ProjectRequest
{
    public class Edit
    {
        /// <summary>
        /// The unique identifier of the project.
        /// </summary>
        public required int ProjectId { get; set; }
        
        /// <summary>
        /// The name of the project.
        /// </summary>
        public required string Name { get; set; }
        
        public class Validator : AbstractValidator<Edit>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(250); 
            }
        }
    }
}