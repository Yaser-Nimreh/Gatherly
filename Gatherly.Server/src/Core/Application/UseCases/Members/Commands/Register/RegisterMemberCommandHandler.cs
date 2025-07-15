using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Domain.Results;
using Domain.ValueObjects;

namespace Application.UseCases.Members.Commands.Register;

public sealed class RegisterMemberCommandHandler(
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(RegisterMemberCommand command, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(command.FirstName);

        if (firstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(command.LastName);

        if (lastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(lastNameResult.Error);
        }

        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        var firstName = firstNameResult.Value;
        var lastName = lastNameResult.Value;
        var email = emailResult.Value;

        var isEmailUnique = await _memberRepository.IsEmailUniqueAsync(email, cancellationToken);

        var memberResult = Member.Register(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            isEmailUnique);

        if (memberResult.IsFailure)
        {
            return Result.Failure<Guid>(memberResult.Error);
        }

        var member = memberResult.Value;

        _memberRepository.Add(member);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}