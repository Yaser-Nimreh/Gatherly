using System.Reflection;

namespace Domain.Primitives;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<Dictionary<int, TEnum>> Enumerations = new(() => CreateEnumerations(typeof(TEnum)));

    protected Enumeration(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    protected Enumeration() => Name = string.Empty;

    public int Id { get; protected init; }
    public string Name { get; protected init; }

    public static IReadOnlyCollection<TEnum> GetValues() => [.. Enumerations.Value.Values];

    public static TEnum? FromId(int id) => Enumerations.Value.TryGetValue(id, out var enumeration) ? enumeration : default;

    public static TEnum? FromName(string name) => Enumerations.Value.Values.SingleOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));

    public static bool Contains(int id) => Enumerations.Value.ContainsKey(id);

    private static Dictionary<int, TEnum> CreateEnumerations(Type enumType) => GetFieldsForType(enumType).ToDictionary(t => t.Id);

    private static IEnumerable<TEnum> GetFieldsForType(Type enumType) =>
        enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

    public bool Equals(Enumeration<TEnum>? other) => other is not null && GetType() == other.GetType() && other.Id.Equals(Id);

    public override bool Equals(object? obj) => obj is Enumeration<TEnum> other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Enumeration<TEnum>? first, Enumeration<TEnum>? second) =>
        first is not null && second is not null && Equals(first, second);

    public static bool operator !=(Enumeration<TEnum>? first, Enumeration<TEnum>? second) =>
        !(first == second);

    public override string ToString() => Name;
}