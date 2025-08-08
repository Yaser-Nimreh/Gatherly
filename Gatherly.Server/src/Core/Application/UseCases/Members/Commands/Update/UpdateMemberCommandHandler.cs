using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;
using Domain.ValueObjects;

namespace Application.UseCases.Members.Commands.Update;

internal sealed class UpdateMemberCommandHandler(
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMemberCommand>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        var _member = await _memberRepository.GetByIdAsync(command.MemberId, cancellationToken);

        if (_member is null)
        {
            return Result.Failure(MemberErrors.NotFound(command.MemberId));
        }

        var firstNameResult = FirstName.Create(command.FirstName);

        if (firstNameResult.IsFailure)
        {
            return Result.Failure(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(command.LastName);

        if (lastNameResult.IsFailure)
        {
            return Result.Failure(lastNameResult.Error);
        }

        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        var firstName = firstNameResult.Value;
        var lastName = lastNameResult.Value;
        var email = emailResult.Value;

        var isEmailUnique = await _memberRepository.IsEmailUniqueAsync(email, cancellationToken)
            || _member.Email!.Value.Equals(command.Email, StringComparison.OrdinalIgnoreCase);
        
        var memberResult = _member.Update(
            firstName,
            lastName,
            email,
            isEmailUnique);

        if (memberResult.IsFailure)
        {
            return Result.Failure(memberResult.Error);
        }

        var member = memberResult.Value;

        _memberRepository.Update(member);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}