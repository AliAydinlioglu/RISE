using Microsoft.AspNetCore.Components;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

public partial class ContactCommunication
{
    [Parameter] public IEnumerable<ContactDto.CommunicationChannelDto> Channels { get; set; } = [];
}
