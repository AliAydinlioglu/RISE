using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Rise.Client.Components;


public partial class RiseSwipeNavigationWrapper : ComponentBase
{
    private double _pointerStartX;
    private double _pointerStartY;
    private double _pointerEndX;
    private double _pointerEndY;
    private bool _isHorizontalSwipe;
    private bool _isPointerDown;
    private const int SwipeThreshold = 75;

    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public string? SwipeLeftUrl { get; set; }
    [Parameter] public string? SwipeRightUrl { get; set; }
    [Parameter] public EventCallback OnSwipeLeft { get; set; }
    [Parameter] public EventCallback OnSwipeRight { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    private void HandlePointerDown(PointerEventArgs e)
    {
        if (e.PointerType == "mouse") return;

        _pointerStartX = e.ClientX;
        _pointerStartY = e.ClientY;
        _pointerEndX = _pointerStartX;
        _pointerEndY = _pointerStartY;
        _isHorizontalSwipe = false;
        _isPointerDown = true;
    }

    private void HandlePointerMove(PointerEventArgs e)
    {
        if (!_isPointerDown || e.PointerType == "mouse") return;

        _pointerEndX = e.ClientX;
        _pointerEndY = e.ClientY;

        var deltaX = Math.Abs(_pointerEndX - _pointerStartX);
        var deltaY = Math.Abs(_pointerEndY - _pointerStartY);

        // If horizontal movement is greater than 2x vertical, it's a horizontal swipe
        if (deltaX > 30 && deltaX > deltaY * 2)
        {
            _isHorizontalSwipe = true;
        }
    }

    private async Task HandlePointerUp(PointerEventArgs e)
    {
        if (!_isPointerDown || e.PointerType == "mouse") return;

        if (_isHorizontalSwipe)
        {
            var deltaX = _pointerEndX - _pointerStartX;

            if (Math.Abs(deltaX) > SwipeThreshold)
            {
                // Swipe left 
                if (deltaX < 0)
                {
                    if (!string.IsNullOrEmpty(SwipeLeftUrl))
                    {
                        NavigationManager.NavigateTo(SwipeLeftUrl);
                    }
                    else if (OnSwipeLeft.HasDelegate)
                    {
                        await OnSwipeLeft.InvokeAsync();
                    }
                }
                // Swipe right 
                else if (deltaX > 0)
                {
                    if (!string.IsNullOrEmpty(SwipeRightUrl))
                    {
                        NavigationManager.NavigateTo(SwipeRightUrl);
                    }
                    else if (OnSwipeRight.HasDelegate)
                    {
                        await OnSwipeRight.InvokeAsync();
                    }
                }
            }
        }

        HandlePointerCancel();
    }

    private void HandlePointerCancel()
    {
        _isHorizontalSwipe = false;
        _isPointerDown = false;
    }
}
