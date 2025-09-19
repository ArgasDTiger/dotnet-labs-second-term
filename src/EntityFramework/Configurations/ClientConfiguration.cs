using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace EntityFramework.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder
            .HasKey(c => c.Id);

        builder
            .HasIndex(c => c.Id);

        builder
            .Property(c => c.FirstName)
            .HasMaxLength(50);

        builder
            .Property(c => c.MiddleName)
            .HasMaxLength(50);

        builder
            .Property(c => c.LastName)
            .HasMaxLength(50);

        builder
            .Property(c => c.PhoneNumber)
            .HasMaxLength(15);

        builder
            .Property(c => c.HomeAddress)
            .HasMaxLength(255);

        builder
            .Property(c => c.PassportSeries)
            .IsRequired(false)
            .HasMaxLength(2);

        builder
            .Property(c => c.PassportNumber)
            .HasMaxLength(9);

        builder
            .HasMany(c => c.ClientMovies)
            .WithOne(cm => cm.Client)
            .HasForeignKey(cm => cm.ClientId);
    }
}