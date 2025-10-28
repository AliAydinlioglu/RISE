using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Rise.Client.Components;

public partial class RiseCarousel
{
    private double _touchStartX;
    private double _touchEndX;
    private int _currentIndex;

    [Parameter, EditorRequired] public List<RenderFragment> Items { get; set; } = new();

    [Parameter] public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (_currentIndex != value)
            {
                _currentIndex = value;
                CurrentIndexChanged.InvokeAsync(value);
            }
        }
    }

    [Parameter]
    public EventCallback<int> CurrentIndexChanged { get; set; }

    [Parameter]
    public int SwipeThreshold { get; set; } = 50;

    private void NavigateToIndex(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            CurrentIndex = index;
            StateHasChanged();
        }
    }

    private void HandleTouchStart(TouchEventArgs e)
    {
        _touchStartX = e.Touches[0].ClientX;
        _touchEndX = _touchStartX;
    }

    private void HandleTouchMove(TouchEventArgs e)
    {
        _touchEndX = e.Touches[0].ClientX;
    }

    private void HandleTouchEnd()
    {
        var deltaX = _touchEndX - _touchStartX;
        
        if (Math.Abs(deltaX) > SwipeThreshold)
        {
            if (deltaX < 0 && _currentIndex < Items.Count - 1)
                CurrentIndex++;
            else if (deltaX > 0 && _currentIndex > 0)
                CurrentIndex--;
        }

        StateHasChanged();
    }
}