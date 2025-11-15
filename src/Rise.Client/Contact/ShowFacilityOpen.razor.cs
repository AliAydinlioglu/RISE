using Microsoft.AspNetCore.Components;
using static Rise.Shared.Contact.ContactDto;

namespace Rise.Client.Contact;

public partial class ShowFacilityOpen
{
    [Parameter] public bool? IsFacilityOpen { get; set; }
}
