using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.ToTable(TableNames.Attendees);

        builder.HasKey(attendee => new { attendee.GatheringId, attendee.MemberId });

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}