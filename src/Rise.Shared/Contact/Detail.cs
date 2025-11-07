namespace Rise.Shared.Contact;

public static partial class ContactRequest
{
    public class Detail
    {
        public int Id { get; set; }
    }
}

public static partial class ContactResponse
{
    public class Detail
    {
        public required ContactDto.Index Service { get; set; }
    }
}

