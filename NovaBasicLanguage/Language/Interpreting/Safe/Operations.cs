namespace NovaBASIC.Language.Interpreting.Safe;

public static class Operations
{
    public static object Add(object lhs, object rhs)
    {
        if (lhs is string || rhs is string)
        {
            var lhsString = lhs as string ?? "null";
            var rhsString = rhs as string ?? "null";
            return lhsString + rhsString;
        }

        if (lhs == null)
        {
            return rhs ?? "null";
        }

        if (rhs == null)
        {
            return lhs;
        }

        if (TryAddAsDecimals(lhs, rhs, out var decimalVar))
        {
            return decimalVar;
        }

        return lhs.ToString() + rhs.ToString();
    }

    private static bool TryAddAsDecimals(object lhs, object rhs, out decimal result)
    {
        result = 0;
        try
        {
            result = (dynamic)lhs + (dynamic)rhs;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool ToBool(object? value)
    {
        if (value is null)
        {
            return false;
        }

        return value switch
        {
            int intValue => intValue != 0,
            string stringValue => bool.TryParse(stringValue, out bool parsedString) && parsedString,
            bool boolValue => boolValue,
            _ => throw new InvalidCastException($"Unable to cast typeof '{value.GetType()}' to bool."),
        };
    }

    public static bool Equal(dynamic lhs, dynamic rhs)
    {
        if(lhs is null && rhs is null)
        {
            return true;
        }

        if(lhs is null || rhs is null)
        {
            return false;
        }

        return lhs.Equals(rhs);
    }
}
