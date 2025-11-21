using Microsoft.AspNetCore.Components;
using Rise.Domain.Contact;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

public partial class ContactCommunication
{
    [Parameter] public IEnumerable<ContactDto.CommunicationChannelDto> Channels { get; set; } = [];

    private string GetIconForCommunicationType(CommunicationTypes type)
    {
        return type switch
        {
            CommunicationTypes.Email => "fa-envelope",
            CommunicationTypes.Phone => "fa-phone",
            CommunicationTypes.Form => "fa-external-link-alt",
            CommunicationTypes.SocialMedia => "fa-share-alt",
            _ => "fa-link"
        };
    }
}
