using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace EntityFramework.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.
            HasMany(c => c.ClientMovies).
            WithOne(cm => cm.Movie).
            HasForeignKey(cm => cm.MovieId);
    }
}