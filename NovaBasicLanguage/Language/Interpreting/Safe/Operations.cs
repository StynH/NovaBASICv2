namespace NovaBASIC.Language.Interpreting.Safe;

public static class Operations
{
    public static object Add(object lhs, object rhs)
    {
        if (lhs == null)
        {
            return rhs ?? "";
        }

        if (rhs == null)
        {
            return lhs;
        }

        if (lhs is string || rhs is string)
        {
            return lhs.ToString() + rhs.ToString();
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
        if (decimal.TryParse(lhs.ToString(), out var decimal1) &&
            decimal.TryParse(rhs.ToString(), out var decimal2))
        {
            result = decimal1 + decimal2;
            return true;
        }
        return false;
    }
}
