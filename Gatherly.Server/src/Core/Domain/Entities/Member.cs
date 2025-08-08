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

    private Member() : base() { }

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

        member.Raise(new MemberRegisteredEvent(Guid.NewGuid(), member.Id));

        return member;
    }

    private void ChangeName(FirstName firstName, LastName lastName)
    {
        if (!FirstName!.Equals(firstName) || !LastName!.Equals(lastName))
        {
            Raise(new MemberNameChangedEvent(Guid.NewGuid(), Id));
        }

        FirstName = firstName;
        LastName = lastName;
    }

    private void ChangeEmail(Email email) 
    {
        if (!Email!.Equals(email)) 
        {
            Raise(new MemberEmailChangedEvent(Guid.NewGuid(), Id));
        }

        Email = email;
    }

    public Result<Member> Update(FirstName firstName, LastName lastName, Email email, bool isEmailUnique)
    {
        if (!isEmailUnique)
        {
            return Result.Failure<Member>(MemberErrors.DuplicateEmail(email.ToString()));
        }

        ChangeName(firstName, lastName);

        ChangeEmail(email);

        return this;
    }

    public MemberSnapshot ToSnapshot()
    {
        return new MemberSnapshot()
        {
            Id = Id,
            FirstName = FirstName!.Value,
            LastName = LastName!.Value,
            Email = Email!.Value,
            CreatedAt = CreatedAt,
            LastUpdatedAt = LastUpdatedAt
        };
    }

    public static Member FromSnapshot(MemberSnapshot memberSnapshot)
    {
        return new Member(
            memberSnapshot.Id,
            FirstName.Create(memberSnapshot.FirstName).Value,
            LastName.Create(memberSnapshot.LastName).Value,
            Email.Create(memberSnapshot.Email).Value)
        {
            CreatedAt = memberSnapshot.CreatedAt,
            LastUpdatedAt = memberSnapshot.LastUpdatedAt
        };
    }
}