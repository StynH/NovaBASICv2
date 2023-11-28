using NovaBASIC.Language.Lexicon;
using System.Globalization;

namespace NovaBASIC.Extensions;

public static class StringExtensions
{
    public static bool IsArithmetic(this string value)
    {
        return value.Equals(Tokens.PLUS) || value.Equals(Tokens.MINUS) || value.Equals(Tokens.DIVIDE) || value.Equals(Tokens.MODULO) || value.Equals(Tokens.MULTIPLY);
    }

    public static bool IsEqualityCheck(this string value)
    {
        return value.Equals(Tokens.EQUALS) || value.Equals(Tokens.NOT_EQUALS) || value.Equals(Tokens.GT) || value.Equals(Tokens.GTE) || value.Equals(Tokens.LT) || value.Equals(Tokens.LTE);
    }

    public static bool IsNumeric(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }
}
