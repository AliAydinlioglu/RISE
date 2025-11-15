using Microsoft.AspNetCore.Components;
using static Rise.Shared.Contact.ContactDto;

namespace Rise.Client.Contact;

public partial class ContactOpeningHours
{
    [Parameter, EditorRequired] public IEnumerable<ContactPeriodDto> OpeningHours { get; set; } = [];
}
