namespace Rise.Shared.Contact;

public static partial class ContactResponse
{
    public class Index
    {
        public IEnumerable<ContactDto.Index> Facilities { get; set; } = [];
        public int TotalCount { get; set; }
    }
}

