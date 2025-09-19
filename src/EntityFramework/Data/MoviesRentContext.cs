using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Settings;

namespace EntityFramework.Data;

public sealed class MoviesRentContext : DbContext
{
    private readonly ConnectionStringSettings _connectionStringSettings;

    public MoviesRentContext(
        DbContextOptions<MoviesRentContext> options,
        IOptions<ConnectionStringSettings> connectionStringSettings)
        : base(options)
    {
        _connectionStringSettings = connectionStringSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionStringSettings.Default);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}