using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace EntityFramework.Configurations;

public sealed class ClientMovieConfiguration : IEntityTypeConfiguration<ClientMovie>
{
    public void Configure(EntityTypeBuilder<ClientMovie> builder)
    {
        builder.ToTable("ClientMovie");

        builder
            .HasKey(cm => cm.Id);

        builder
            .HasIndex(cm => cm.Id);

        builder
            .HasIndex(cm => new { cm.ClientId, cm.MovieId });

        builder
            .HasOne(cm => cm.Client)
            .WithMany(c => c.ClientMovies)
            .HasForeignKey(cm => cm.ClientId);

        builder
            .HasOne(cm => cm.Movie)
            .WithMany(m => m.ClientMovies)
            .HasForeignKey(cm => cm.MovieId);
    }
}