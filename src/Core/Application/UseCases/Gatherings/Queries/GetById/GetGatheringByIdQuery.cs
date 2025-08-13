using Application.Abstractions.Messaging;
using Application.UseCases.Gatherings.Responses;

namespace Application.UseCases.Gatherings.Queries.GetById;

public record GetGatheringByIdQuery(Guid GatheringId) : IQuery<GatheringResponse>;