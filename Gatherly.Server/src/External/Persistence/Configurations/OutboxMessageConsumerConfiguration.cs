using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;
using Persistence.Outbox;

namespace Persistence.Configurations;

internal sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable(TableNames.OutboxMessageConsumers);

        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });
    }
}