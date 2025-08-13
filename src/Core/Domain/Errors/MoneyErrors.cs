using Domain.Results;

namespace Domain.Errors;

public static class MoneyErrors
{
    public static readonly Error NegativeAmount = Error.Failure(
        "Money.NegativeAmount",
        "Amount cannot be negative.");

    public static readonly Error EmptyCurrency = Error.Failure(
        "Money.EmptyCurrency",
        "Currency cannot be null or empty.");

    public static readonly Error InvalidCurrencyLength = Error.Failure(
        "Money.InvalidCurrencyLength",
        "Currency must be a valid 3-letter ISO code.");

    public static readonly Error DifferentCurrencies = Error.Failure(
        "Money.DifferentCurrencies",
        "Cannot operate on money with different currencies.");

    public static readonly Error NegativeResult = Error.Failure(
        "Money.NegativeResult",
        "Resulting amount cannot be negative.");
}
