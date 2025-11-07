using Microsoft.AspNetCore.Components;
using static Rise.Shared.Contact.ContactDto;

namespace Rise.Client.Contact;

public partial class ContactLocation
{
    [Parameter]public string ExtraInfo { get ; set; } = string.Empty;
    [Parameter] public ServiceLocationDto? Location { get; set; }

    public bool HasAddress()
    {
        return Location != null && Location.ServiceAddress != null;
    }
}
