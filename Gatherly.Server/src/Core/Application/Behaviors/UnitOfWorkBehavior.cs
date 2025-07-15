using Application.Abstractions.Messaging;
using Domain.Abstractions;
using MediatR;

namespace Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Bypass transaction logic for Queries
        if (request is not ICommand && !request.GetType().GetInterfaces().Any(x =>
            x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommand<>)))
        {
            return await next(cancellationToken);
        }

        TResponse? response = default;

        await _unitOfWork.ExecuteWithTransactionAsync(async cancellationToken =>
        {
            response = await next(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        return response!;
    }
}