using EntityFramework.Data;
using EntityFramework.Data.Seed;
using EntityFramework.Extensions;
using Shared.Extensions;
using AdoNet.Extensions;
using EntityFramework.Api.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSharedServices(builder.Configuration);

if (args.Contains("ado", StringComparer.OrdinalIgnoreCase))
{
    builder.Services.AddAdoServices();
} 
else
{
    builder.Services.AddEntityFramework();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();
app.MapEndpoints();

if (args.Contains("seed", StringComparer.OrdinalIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MoviesRentContext>();
    var seeder = new Seeder(context);
    await seeder.SeedAsync();
}

app.Run();