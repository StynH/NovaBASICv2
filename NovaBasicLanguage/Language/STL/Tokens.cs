namespace NovaBASIC.Language.Lexicon;

public static partial class Tokens
{
    // STL
    public const string INCREMENT_STL = "INCREMENT";
    public const string DECREMENT_STL = "DECREMENT";
    public const string MULTIPLY_STL = "MULTIPLY";
    public const string DIVIDE_STL = "DIVIDE";
    public const string PRINT_STL = "PRINT";
    public const string MATCHES_STL = "MATCHES";
    public const string ARRAY_RESIZE_STL = "ARRAY_RESIZE";
    public const string COUNT_STL = "COUNT";

    // Math
    public const string RAND_STL = "RAND";
    public const string FLOOR_STL = "FLOOR";
    public const string CEIL_STL = "CEIL";
    public const string SIN_STL = "SIN";
    public const string COS_STL = "COS";
    public const string TAN_STL = "TAN";
    public const string TO_ARRAY_STL = "TO_ARRAY";

    // Patterns
    public const string FLOAT_PATTERN = "\\d+\\.\\d+";
    public const string AZ09_PATTERN = "[a-zA-Z0-9_]+";
    public const string LITERALS_PATTERN = "\"([^\"]+)\"";
}
