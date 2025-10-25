namespace Rise.Client.Components;

public partial class RiseLoader
{
    private static char[] Letters => "HOGENT".ToCharArray();
    private string AnimationState { get; set; } = "initial";
    
    private int _rotationOffset;
    private bool _isLettersMovingToCentre;

    protected override async Task OnInitializedAsync()
    {
        await AnimateLoader();
    }

    private async Task AnimateLoader()
    {
        while (true)
        {
            await AnimateToCenter();
            _rotationOffset = (_rotationOffset + 1) % Letters.Length;
            await AnimateToOutside();
            await PrepareAnimationIteration();
        }
    }

    private async Task AnimateToCenter()
    {
        AnimationState = "to-center";
        _isLettersMovingToCentre = true;
        
        await InvokeAsync(StateHasChanged);
        await Task.Delay(700);
        
        _isLettersMovingToCentre = false;
    }

    private async Task AnimateToOutside()
    {
        AnimationState = "to-outside";
        await InvokeAsync(StateHasChanged);
        await Task.Delay(700);
    }

    private async Task PrepareAnimationIteration()
    {
        AnimationState = "initial";
        await InvokeAsync(StateHasChanged);
        await Task.Delay(700);
    }

    private int GetLetterPosition(int index)
    {
        return (index + _rotationOffset) % Letters.Length;
    }

    private string GetPositionAnimationStyle(int position)
    {
        if (_isLettersMovingToCentre)
        {
            return "top: 50%; left: 50%; transform: translate(-50%, -50%);";
        }

        return position switch
        {
            0 => "top: 0; left: 50%; transform: translate(-50%, 0);",       // BOVEN
            1 => "top: 30%; left: 80%; transform: translate(-50%, -50%);",  // RECHTS BOVEN
            2 => "top: 60%; left: 80%; transform: translate(-50%, -50%);",  // RECHTS ONDER
            3 => "top: 85%; left: 50%; transform: translate(-50%, -50%);",  // ONDER
            4 => "top: 60%; left: 20%; transform: translate(-50%, -50%);",  // LINKS ONDER
            5 => "top: 30%; left: 20%; transform: translate(-50%, -50%);",  // LINKS BOVEN
        };
    }
}