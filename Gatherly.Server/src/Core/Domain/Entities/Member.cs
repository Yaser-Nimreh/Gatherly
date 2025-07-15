using Domain.Errors;
using Domain.Events.Members;
using Domain.Primitives;
using Domain.Results;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Member : AggregateRoot
{
    private Member(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private Member() : base(Guid.Empty) { }

    public FirstName? FirstName { get; private set; }
    public LastName? LastName { get; private set; }
    public Email? Email { get; private set; }

    public static Result<Member> Register(Guid id, FirstName firstName, LastName lastName, Email email, bool isEmailUnique)
    {
        if (!isEmailUnique)
        {
            return Result.Failure<Member>(MemberErrors.DuplicateEmail(email.ToString()));
        }

        var member = new Member(id, firstName, lastName, email);

        member.Raise(new MemberRegisteredEvent(member.Id));

        return member;
    }
}