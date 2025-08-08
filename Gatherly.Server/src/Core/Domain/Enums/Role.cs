using Domain.Primitives;

namespace Domain.Enums;

public abstract class Role : Enumeration<Role>
{
    public static readonly Role Registered = new RegisteredRole();

    protected Role(int id, string name)
        : base(id, name) { }

    protected Role() { }

    public abstract string Description { get; }
    public abstract bool IsSystemRole { get; }

    private sealed class RegisteredRole : Role
    {
        public RegisteredRole() : base(1, nameof(Registered)) { }

        public override string Description => "Default role assigned to newly registered users.";
        public override bool IsSystemRole => true;
    }
}