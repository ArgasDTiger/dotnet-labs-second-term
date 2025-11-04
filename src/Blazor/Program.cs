using Blazor.Clients.Services;
using Blazor.Movies.Services;
using Blazor.Shared;
using Blazor.Shared.Components.Modal;
using Blazor.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddApiHttpClient<IMoviesService, MoviesService>(builder.Configuration);
builder.Services.AddApiHttpClient<IClientsService, ClientsService>(builder.Configuration);

builder.AddModal();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();