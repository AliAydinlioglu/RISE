namespace Rise.Client.Calendar.Components;

public partial class CalendarDesktop
{
    private DateTime _dateInView;

    protected override void OnParametersSet()
    {
        _dateInView = _selectedDate;
    }
}