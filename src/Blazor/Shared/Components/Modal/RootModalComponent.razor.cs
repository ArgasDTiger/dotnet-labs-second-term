using Microsoft.AspNetCore.Components;

namespace Blazor.Shared.Components.Modal;

public sealed partial class RootModalComponent : ComponentBase, IDisposable
{
    [Inject] private IModalService ModalService { get; init; } = null!;

    protected override void OnInitialized()
    {
        ModalService.OnUpdate += StateHasChanged;
    }

    public void Dispose()
    {
        ModalService.OnUpdate -= StateHasChanged;
    }
}