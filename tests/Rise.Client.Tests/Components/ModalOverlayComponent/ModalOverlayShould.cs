using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

namespace Rise.Client.Components.ModalOverlayComponent;
using Shouldly;

public class ModalOverlayShould : MudBlazorTestSetup
{
    
    [Fact]
    public void RendersTitleChildAndVisibleClass_WhenModalIsOpen()
    {
        var cut = RenderComponent<RiseModalOverlay>(parameters => parameters
            .Add(p => p.Visible, true)
            .Add(p => p.Title, "My Title")
            .AddChildContent("<div class='child'>Child content</div>")
        );

        Assert.Contains("modal-overlay visible", cut.Markup);
        Assert.Equal("My Title", cut.Find("h3").TextContent);
        Assert.Contains("Child content", cut.Markup);
    }

    [Fact]
    public void InvokeOnClose_WhenBackButtonGetsClicked()
    {
        var closed = false;

        var cut = RenderComponent<RiseModalOverlay>(parameters => parameters
            .Add(p => p.Visible, true)
            .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closed = true))
        );

        var backLink = cut.Find("[data-bunit='modal-back-button']");
        backLink.Click();

        closed.ShouldBeTrue();
        
    }
}