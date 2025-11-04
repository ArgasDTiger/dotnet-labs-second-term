namespace Blazor.Shared.Components.Modal;

public static class ModalExtension
{
    public static void AddModal(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IModalService, ModalService>();
    }
}