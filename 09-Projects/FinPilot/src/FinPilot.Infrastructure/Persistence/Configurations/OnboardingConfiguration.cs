using FinPilot.Domain.Onboarding.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.Infrastructure.Persistence.Configurations;

public sealed class OnboardingConfiguration : IEntityTypeConfiguration<OnboardingAggregate>
{
    public void Configure(EntityTypeBuilder<OnboardingAggregate> builder)
    {
        builder.ToTable("onboardings");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.IdentityNumber)
            .HasConversion(
                valueObject => valueObject.Value,
                value => IdentityNumber.Create(value))
                .HasColumnName("identity_number")
                .HasMaxLength(11)
                .IsRequired();

        builder.Property(o => o.Email)
            .HasConversion(
                valueObject => valueObject.Value,
                value => Email.Create(value))
            .HasColumnName("email")
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(o => o.PhoneNumber)
            .HasConversion(
                valueObject => valueObject.Value,
                value => PhoneNumber.Create(value))
            .HasColumnName("phone_number")
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(o => o.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();
    }
}