using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableNames.Roles);

        builder.HasKey(role => role.Id);

        builder.Property(role => role.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(role => role.NormalizedName)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(role => role.NormalizedName).IsUnique();

        builder.Property(role => role.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(role => role.IsSystemRole)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(role => role.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(role => role.Users)
            .WithMany(user => user.Roles);
    }
}