using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable(TableNames.Invitations);

        builder.HasKey(invitation => invitation.Id);

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(invitation => invitation.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}