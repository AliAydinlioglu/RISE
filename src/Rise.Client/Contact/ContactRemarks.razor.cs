using Microsoft.AspNetCore.Components;

namespace Rise.Client.Contact;

public partial class ContactRemarks
{
    [Parameter, EditorRequired] public IEnumerable<string> Remarks { get; set; } = [];
}
