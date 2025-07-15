using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(member => member.Id);

        builder
            .Property(member => member.FirstName)
            .HasConversion(firstName => firstName!.Value, value => FirstName.Create(value).Value)
            .HasMaxLength(FirstName.MaxLength);

        builder
            .Property(member => member.LastName)
            .HasConversion(lastName => lastName!.Value, value => LastName.Create(value).Value)
            .HasMaxLength(LastName.MaxLength);

        builder
            .Property(member => member.Email)
            .HasConversion(email => email!.Value, value => Email.Create(value).Value)
            .HasMaxLength(FirstName.MaxLength);

        builder.HasIndex(member => member.Email).IsUnique();
    }
}