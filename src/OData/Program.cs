using EntityFramework.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Scalar.AspNetCore;
using Shared.Entities;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSharedServices(builder.Configuration);
builder.Services.AddEntityFramework();
builder.Services.AddControllers()
    .AddOData(opt =>
    {
        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<Client>("Clients");
        odataBuilder.EntitySet<Movie>("Movies");
        odataBuilder.EntitySet<ClientMovie>("ClientMovies");
        opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .Count()
            .SetMaxTop(100);
    });
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();