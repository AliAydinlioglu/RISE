using Microsoft.AspNetCore.Components;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant.Components;

public partial class AllergenList : ComponentBase
{
    [Parameter] public required IEnumerable<FoodRestrictionDto> Allergens { get; set; } = [];
}