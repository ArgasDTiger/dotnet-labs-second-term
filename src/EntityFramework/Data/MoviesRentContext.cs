using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace EntityFramework.Data;

public sealed class MoviesRentContext : DbContext
{
    public MoviesRentContext(DbContextOptions<MoviesRentContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<ClientMovie> ClientMovie { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}