using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using Shared.Extensions;
using static Shared.Entities.Client;

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
            .HasField(nameof(Client.FirstName).ToCamelCaseWithUnderscore())
            .HasMaxLength(FirstNameMaxLength);

        builder
            .Property(c => c.MiddleName)
            .HasField(nameof(Client.MiddleName).ToCamelCaseWithUnderscore())
            .HasMaxLength(MiddleNameMaxLength);

        builder
            .Property(c => c.LastName)
            .HasField(nameof(Client.LastName).ToCamelCaseWithUnderscore())
            .HasMaxLength(LastNameMaxLength);

        builder
            .Property(c => c.PhoneNumber)
            .HasField(nameof(Client.PhoneNumber).ToCamelCaseWithUnderscore())
            .HasMaxLength(PhoneNumberMaxLength);

        builder
            .Property(c => c.HomeAddress)
            .HasField(nameof(Client.HomeAddress).ToCamelCaseWithUnderscore())
            .HasMaxLength(HomeAddressMaxLength);

        builder
            .Property(c => c.PassportSeries)
            .HasField(nameof(Client.PassportSeries).ToCamelCaseWithUnderscore())
            .IsRequired(false)
            .HasMaxLength(PassportSeriesMaxLength);

        builder
            .Property(c => c.PassportNumber)
            .HasField(nameof(Client.PassportNumber).ToCamelCaseWithUnderscore())
            .HasMaxLength(PassportNumberMaxLength);

        builder
            .HasMany(c => c.ClientMovies)
            .WithOne(cm => cm.Client)
            .HasForeignKey(cm => cm.ClientId);
    }
}