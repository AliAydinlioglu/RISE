using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Contact;
using Rise.Shared.Common;

namespace Rise.Client.Contact;

[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Contact", route: "/contact")]
public partial class ContactIndex
{
    private IEnumerable<ContactDto.Index> contactServices = [];
    [Inject] public required IContactService ContactService{ get; set; }
    private int currentPage = 1;
    private int pageSize = 8;
    //private int totalCount = 0;
    //private int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);

    protected override async Task OnInitializedAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = (currentPage - 1) * pageSize,
            Take = pageSize,
        };
        
        var result = await ContactService.GetIndexAsync(request);
        contactServices = result.Value.Services;
    }

}


