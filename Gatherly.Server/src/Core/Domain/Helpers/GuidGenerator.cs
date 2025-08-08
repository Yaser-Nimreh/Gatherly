using Domain.Primitives;

namespace Domain.Helpers;

public static class GuidGenerator
{
    public static Guid FromString(string input)
    {
        byte[] hash = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(input));

        return new Guid(hash[..16]); // Truncate SHA256 to 128 bits
    }

    public static Guid FromEnum<TEnum>(TEnum value) where TEnum : Enum
    {
        int enumValue = (int)(object)value;
        string input = $"{typeof(TEnum).Name}-{enumValue}";
        return FromString(input);
    }

    public static Guid FromEnumeration<TEnum>(TEnum value) where TEnum : Enumeration<TEnum>
    {
        string input = $"{typeof(TEnum).Name}-{value.Id}";
        return FromString(input);
    }
}