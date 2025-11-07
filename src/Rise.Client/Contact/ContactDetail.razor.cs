using Microsoft.AspNetCore.Components;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

public partial class ContactDetail
{
    private ContactDto.Index _contactService { get; set; }
    [Parameter] public string Id { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return;
        }
        var contactServices = await ContactDataLoader.LoadContactsAsync();
        _contactService = contactServices.Find(c => c.Id == int.Parse(Id));
    }
}
