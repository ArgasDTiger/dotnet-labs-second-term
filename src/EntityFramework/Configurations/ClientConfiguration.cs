using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace EntityFramework.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasMany(c => c.ClientMovies).WithOne(cm => cm.Client).HasForeignKey(cm => cm.ClientId);
        
        builder.Property(c => c.PhoneNumber).HasMaxLength(15);

        builder
            .Property(c => c.PassportSeries)
            .IsRequired(false)
            .HasMaxLength(2);

        builder
            .Property(c => c.PassportNumber)
            .HasMaxLength(9);
    }
}