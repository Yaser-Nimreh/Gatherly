using Domain.Abstractions;
using Domain.Errors;
using Domain.Events.Users;
using Domain.Results;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User : IdentityUser<Guid>, IAggregateRoot, ISoftDeletableEntity, IAuditableEntity, IEntity
{
    private FirstName? _firstName;
    private LastName? _lastName;
    private Email? _email;
    private UserName? _userName;
    private PhoneNumber? _phoneNumber;
    private ProfilePicturePath? _profilePicturePath;

    private User() : base() { } // For EF Core

    private User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email,
        string HashedPassword,
        UserName userName,
        PhoneNumber phoneNumber,
        ProfilePicturePath? profilePicturePath = null)
    {
        Id = id;
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
        PasswordHash = HashedPassword;
        SetUserName(userName);
        SetPhoneNumber(phoneNumber);
        SetProfilePicturePath(profilePicturePath);
    }

    // Exposed ValueObjects
    public FirstName FirstName => _firstName!;
    public LastName LastName => _lastName!;
    public string FullName => $"{_firstName} {_lastName}";
    public Email EmailVO => _email!;
    public UserName UserNameVO => _userName!;
    public PhoneNumber PhoneNumberVO => _phoneNumber!;
    public ProfilePicturePath? ProfilePicturePath => _profilePicturePath!;

    public DateTime? LastLoginAt { get; private set; }

    public ICollection<Role> Roles { get; set; } = [];

    // Setters that sync with IdentityUser base string properties
    private void SetFirstName(FirstName firstName) => _firstName = firstName;
    private void SetLastName(LastName lastName) => _lastName = lastName;

    private void SetEmail(Email email)
    {
        _email = email;
        Email = email.Value;
        NormalizedEmail = email.Value.ToUpperInvariant();
    }

    private void SetUserName(UserName userName)
    {
        _userName = userName;
        UserName = userName.Value;
        NormalizedUserName = userName.Value.ToUpperInvariant();
    }

    private void SetPhoneNumber(PhoneNumber phoneNumber)
    {
        _phoneNumber = phoneNumber;
        PhoneNumber = phoneNumber.Value;
    }

    private void SetProfilePicturePath(ProfilePicturePath? profilePicturePath) => _profilePicturePath = profilePicturePath;

    public void UpdateLastLoginTime() => LastLoginAt = DateTime.UtcNow;

    // EF Core needs to map these base properties directly
    // Email, PhoneNumber, UserName already exist in base class (as string)

    public static Result<User> Register(
        Guid id, 
        FirstName firstName, 
        LastName lastName,
        Email email,
        string HashedPassword,
        UserName userName,
        PhoneNumber phoneNumber,
        bool isEmailUnique,
        ProfilePicturePath? profilePicturePath = null)
    {
        if (!isEmailUnique)
        {
            return Result.Failure<User>(UserErrors.DuplicateEmail(email.ToString()));
        }

        var user = new User(id, firstName, lastName, email, HashedPassword, userName, phoneNumber, profilePicturePath);

        user.Raise(new UserRegisteredEvent(Guid.NewGuid(), user.Id));

        return user;
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public string ItemType => GetType().Name;

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedById { get; private set; }
    public string? DeletedByName { get; private set; }

    public void Delete(Guid? deletedById = null, string? deletedByName = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedById = deletedById;
        DeletedByName = deletedByName;
    }

    public void UnDelete()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedById = null;
        DeletedByName = null;
    }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}