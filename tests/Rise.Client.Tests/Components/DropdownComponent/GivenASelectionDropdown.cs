using Rise.Client.Components.Dropdown;

namespace Rise.Client.Components.DropdownComponent;

public class GivenASelectionDropdown : GivenADropdownSetupTest<RiseSelectionDropdown<string>, string>
{
    private static readonly List<string> DefaultOptions = new() { "Option A", "Option B", "Option C" };
    protected override List<string> GetDefaultItems() => DefaultOptions;
    protected override Func<string, string> GetDefaultItemTextFunc() => option => option;

}