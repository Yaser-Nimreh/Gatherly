namespace Domain.Primitives;

public abstract class ValueObject<TValue> : IEquatable<TValue>
    where TValue : ValueObject<TValue>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    private bool ValuesAreEqual(TValue other) => GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public bool Equals(TValue? other) => other is not null && ValuesAreEqual(other);

    public override bool Equals(object? obj) => obj is TValue other && Equals(other);

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Aggregate(1, (hash, component) =>
                HashCode.Combine(hash, component?.GetHashCode() ?? 0));

    public static bool operator ==(ValueObject<TValue>? first, ValueObject<TValue>? second) =>
        ReferenceEquals(first, second) || (first is not null && first.Equals(second));

    public static bool operator !=(ValueObject<TValue>? first, ValueObject<TValue>? second) => !(first == second);
}