namespace Rise.Shared.Identity;

public class UserDto
{
    public string UserId { get; set; }
    public string ClassGroup { get; set; }
    
    public UserDto(string userId, string classGroup)
    {
        UserId = userId;
        ClassGroup = classGroup;
    }
}