using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using static Shared.Constants.Entities.MovieConstants.Validation;

namespace EntityFramework.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder
            .HasKey(m => m.Id);

        builder
            .HasIndex(m => m.Id);

        builder
            .Property(m => m.Title)
            .HasMaxLength(TitleMaxLength);

        builder
            .Property(m => m.Description)
            .HasMaxLength(DescriptionMaxLength);

        builder
            .Property(m => m.CollateralValue)
            .HasColumnType("decimal(5,2)");

        builder
            .Property(m => m.PricePerDay)
            .HasColumnType("decimal(5,2)");

        builder
            .HasMany(c => c.ClientMovies)
            .WithOne(cm => cm.Movie)
            .HasForeignKey(cm => cm.MovieId);
    }
}