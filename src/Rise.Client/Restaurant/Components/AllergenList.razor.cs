using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class AllergenList : ComponentBase
{
    [Parameter] public required ISet<AllergenDTO> Allergens { get; set; }
}