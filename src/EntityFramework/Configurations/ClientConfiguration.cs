using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Shared.Constants.Entities.ClientConstants.Validation;
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
            .HasMaxLength(FirstNameMaxLength);

        builder
            .Property(c => c.MiddleName)
            .HasMaxLength(MiddleNameMaxLength);

        builder
            .Property(c => c.LastName)
            .HasMaxLength(LastNameMaxLength);

        builder
            .Property(c => c.PhoneNumber)
            .HasMaxLength(PhoneNumberMaxLength);

        builder
            .Property(c => c.HomeAddress)
            .HasMaxLength(HomeAddressMaxLength);

        builder
            .Property(c => c.PassportSeries)
            .IsRequired(false)
            .HasMaxLength(PassportSeriesLength);

        builder
            .Property(c => c.PassportNumber)
            .HasMaxLength(PassportNumberMaxLength);

        builder
            .HasMany(c => c.ClientMovies)
            .WithOne(cm => cm.Client)
            .HasForeignKey(cm => cm.ClientId);
    }
}