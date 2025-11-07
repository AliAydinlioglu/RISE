using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Contact", route: "/contact")]
public partial class ContactIndex
{
    private List<ContactDto.Index> contactServices = [];

    protected override async Task OnInitializedAsync()
    {
        contactServices = await ContactDataLoader.LoadContactsAsync();
    }

}


