using Microsoft.AspNetCore.Components;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

public partial class ContactCommunication
{
    [Parameter] public IEnumerable<ContactDto.CommunicationChannelDto> Channels { get; set; } = [];

    private string GetIconForCommunicationType(string type)
    {
        return type.ToUpperInvariant() switch
        {
            "EMAIL" => "fa-envelope",
            "PHONE" => "fa-phone",
            "FORM" => "fa-external-link-alt",
            "SOCIALMEDIA" => "fa-share-alt",
            _ => "fa-link"
        };
    }
}
