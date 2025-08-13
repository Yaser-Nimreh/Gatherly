using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Domain.ValueObjects;

public sealed class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency) =>
        Result.Ensure(
            (amount, currency),
            (m => m.amount >= 0, MoneyErrors.NegativeAmount),
            (m => !string.IsNullOrWhiteSpace(m.currency), MoneyErrors.EmptyCurrency),
            (m => m.currency.Length == 3, MoneyErrors.InvalidCurrencyLength))
        .Map(m => new Money(m.amount, m.currency.ToUpperInvariant()));

    public Result<Money> Add(Money other)
    {
        if (Currency != other.Currency)
        {
            return Result.Failure<Money>(MoneyErrors.DifferentCurrencies);
        }

        return new Money(Amount + other.Amount, Currency);
    }

    public Result<Money> Subtract(Money other)
    {
        if (Currency != other.Currency)
        {
            return Result.Failure<Money>(MoneyErrors.DifferentCurrencies);
        }

        var newAmount = Amount - other.Amount;

        if (newAmount < 0)
        {
            return Result.Failure<Money>(MoneyErrors.NegativeResult);
        }

        return new Money(newAmount, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount} {Currency}";
}