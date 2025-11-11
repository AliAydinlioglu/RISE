using Microsoft.AspNetCore.Components;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

public partial class ContactDetail
{
    private ContactDto.Index contactFacility { get; set; }
    [Parameter] public string Id { get; set; } = string.Empty;
    [Inject] public required IContactService ContactService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return;
        }
        
        var result = await ContactService.GetDetailByIdAsync(int.Parse(Id));
        contactFacility = result.Value.Service;
    }
}
