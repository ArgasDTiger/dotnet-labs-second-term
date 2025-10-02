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
        var clientsEntity = odataBuilder.EntitySet<Client>("Clients").EntityType;
        clientsEntity.Property(c => c.Id).IsNotNavigable();
        // clientsEntity.Property(c => c.ClientMovies).IsNotNavigable();

        var movieEntity = odataBuilder.EntitySet<Movie>("Movies").EntityType;
        movieEntity.Property(m => m.Id).IsNotNavigable();
        // movieEntity.Property(m => m.ClientMovies).IsNotNavigable();

        odataBuilder.EntitySet<ClientMovie>("ClientMovies").EntityType
            .Property(m => m.Id).IsNotNavigable();
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