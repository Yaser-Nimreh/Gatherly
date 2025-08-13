using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal sealed class GatheringConfiguration : IEntityTypeConfiguration<Gathering>
{
    public void Configure(EntityTypeBuilder<Gathering> builder)
    {
        builder.ToTable(TableNames.Gatherings);

        builder.HasKey(gathering => gathering.Id);

        builder.HasQueryFilter(gathering => !gathering.IsDeleted);

        builder
            .HasOne(gathering => gathering.Creator)
            .WithMany();

        builder
            .HasMany(gathering => gathering.Invitations)
            .WithOne()
            .HasForeignKey(invitation => invitation.GatheringId);

        builder
            .HasMany(gathering => gathering.Attendees)
            .WithOne()
            .HasForeignKey(attendee => attendee.GatheringId);
    }
}