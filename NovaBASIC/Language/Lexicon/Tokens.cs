using System.Reflection;
using System.Text.RegularExpressions;

namespace NovaBASIC.Language.Lexicon;

public static class Tokens
{
    // Operators
    public const string PLUS = "+";
    public const string MINUS = "-";
    public const string MULTIPLY = "*";
    public const string DIVIDE = "/";
    public const string MODULO = "%";
    public const string POWER = "^";
    public const string QUESTION_MARK = "?";
    public const string SEMICOLON = ":";
    public const string COMMA = ",";
    public const string GTE = ">=";
    public const string LTE = "<=";
    public const string GT = ">";
    public const string LT = "<";
    public const string EQUALS = "==";
    public const string NOT_EQUALS = "!=";
    public const string SET = "=";
    public const string OPENING_BRACKET = "[";
    public const string CLOSING_BRACKET = "]";
    public const string BOOL_TRUE = "true";
    public const string BOOL_FALSE = "false";
    public const string NULL = "null";
    public const string OR = "||";
    public const string AND = "&&";

    // Syntax
    public const string IF = "IF";
    public const string THEN = "THEN";
    public const string RETURN = "RETURN";
    public const string ENDIF = "ENDIF";
    public const string SUB = "SUB";
    public const string END_SUB = "ENDSUB";
    public const string GUARD = "GUARD";
    public const string END_GUARD = "ENDGUARD";
    public const string ELSE = "ELSE";
    public const string ELSEIF = "ELSEIF";

    public const string DECLARATION_PATTERN = "[a-zA-Z_$][a-zA-Z0-9_$]*";
    public const string OPENING_PARENTHESIS = "(";
    public const string CLOSING_PARENTHESIS = ")";

    public const string LET = "LET";
    public const string WHILE = "WHILE";
    public const string FOR = "FOR";
    public const string TO = "TO";
    public const string STEP = "STEP";
    public const string NEXT = "NEXT";
    public const string GOTO = "GOTO";
    public const string BY = "BY";

    // STL
    public const string INCREMENT_STL = "INCREMENT";
    public const string DECREMENT_STL = "DECREMENT";
    public const string MULTIPLY_STL = "MULTIPLY";
    public const string DIVIDE_STL = "DIVIDE";
    public const string PRINT_STL = "PRINT";
    public const string MATCHES_STL = "MATCHES";
    public const string ARRAY_RESIZE_STL = "ARRAY_RESIZE";
    public const string LENGTH_STL = "LEN";

    // Math
    public const string RANDOM_STL = "RAND";
    public const string FLOOR_STL = "FLOOR";
    public const string CEIL_STL = "CEIL";
    public const string SIN_STL = "SIN";
    public const string COS_STL = "COS";
    public const string TAN_STL = "TAN";

    // Patterns
    public const string AZ09_PATTERN = "[a-zA-Z0-9_]+";
    public const string LITERALS_PATTERN = "\"([^\"]+)\"";

    public static string BuildRegexPattern()
    {
        var fields = typeof(Tokens)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.FieldType == typeof(string))
                     .Select(field => new
                     {
                         field.Name,
                         Value = field.GetValue(null) as string
                     });

        return string.Join("|", fields.Select(f => f.Name.EndsWith("_PATTERN") ? f.Value : Regex.Escape(f.Value!)));
    }
}
