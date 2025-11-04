using Microsoft.AspNetCore.Components;

namespace Blazor.Shared.Components.CardItem;

public sealed partial class CardItem<TItem> : ComponentBase
{
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    [Parameter]
    public RenderFragment? MiddleContent { get; set; }

    [Parameter]
    public RenderFragment? ListContent { get; set; }

    [Parameter]
    public TItem? Item { get; set; }

    [Parameter]
    public bool ShowActions { get; set; } = true;

    [Parameter]
    public bool ShowDetails { get; set; }

    [Parameter]
    public bool ShowEdit { get; set; } = true;

    [Parameter]
    public bool ShowDelete { get; set; } = true;

    [Parameter]
    public EventCallback<TItem> OnDetailsClick { get; set; }

    [Parameter]
    public EventCallback<TItem> OnEditClick { get; set; }

    [Parameter]
    public EventCallback<TItem> OnDeleteClick { get; set; }

    private async Task HandleDetailsClick()
    {
        if (Item != null)
        {
            await OnDetailsClick.InvokeAsync(Item);
        }
    }

    private async Task HandleEditClick()
    {
        if (Item != null)
        {
            await OnEditClick.InvokeAsync(Item);
        }
    }

    private async Task HandleDeleteClick()
    {
        if (Item != null)
        {
            await OnDeleteClick.InvokeAsync(Item);
        }
    }
}