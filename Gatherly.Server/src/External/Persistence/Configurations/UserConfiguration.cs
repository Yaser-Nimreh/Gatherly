using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);

        builder
            .Property(user => user.FirstName)
            .HasConversion(
                firstName => firstName.Value,
                value => FirstName.Create(value).Value)
            .HasMaxLength(FirstName.MaxLength)
            .IsRequired();

        builder
            .Property(user => user.LastName)
            .HasConversion(
                lastName => lastName.Value,
                value => LastName.Create(value).Value)
            .HasMaxLength(LastName.MaxLength)
            .IsRequired();

        // Only map Identity fields
        builder.Property(u => u.Email).HasMaxLength(Email.MaxLength);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(Email.MaxLength);
        builder.Property(u => u.UserName).HasMaxLength(UserName.MaxLength);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(UserName.MaxLength);
        builder.Property(u => u.PhoneNumber).HasMaxLength(PhoneNumber.MaxLength);

        // Ignore public VO properties
        builder.Ignore(u => u.EmailVO);
        builder.Ignore(u => u.UserNameVO);
        builder.Ignore(u => u.PhoneNumberVO);

        builder
            .Property(user => user.ProfilePicturePath)
            .HasConversion(
                profilePicturePath => profilePicturePath == null ? null : profilePicturePath.Value,
                value => value == null ? null : ProfilePicturePath.Create(value).Value)
            .HasField("_profilePicturePath");

        builder
            .Property(user => user.LastLoginAt);

        builder.HasIndex(user => user.Email).IsUnique();
    }
}