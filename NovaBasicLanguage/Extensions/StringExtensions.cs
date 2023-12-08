using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Extensions;
using System.Globalization;

namespace NovaBASIC.Extensions;

public static partial class StringExtensions
{
    public static bool IsArithmetic(this string value)
    {
        return value.Equals(Tokens.PLUS) || value.Equals(Tokens.MINUS) || value.Equals(Tokens.DIVIDE) || value.Equals(Tokens.MODULO) || value.Equals(Tokens.MULTIPLY);
    }

    public static bool IsEqualityCheck(this string value)
    {
        return value.Equals(Tokens.EQUALS) 
            || value.Equals(Tokens.NOT_EQUALS) 
            || value.Equals(Tokens.GT) 
            || value.Equals(Tokens.GTE) 
            || value.Equals(Tokens.LT) 
            || value.Equals(Tokens.LTE);
    }

    public static bool IsOrAnd(this string value)
    {
        return value.Equals(Tokens.AND)
            || value.Equals(Tokens.OR);
    }

    public static bool IsBitwiseOperator(this string value)
    {
        return value.Equals(Tokens.BITWISE_AND)
            || value.Equals(Tokens.BITWISE_OR)
            || value.Equals(Tokens.BITWISE_XOR)
            || value.Equals(Tokens.BITWISE_LEFT_SHIFT)
            || value.Equals(Tokens.BITWISE_RIGHT_SHIFT)
            || value.Equals(Tokens.BITWISE_NOT);
    }

    public static bool IsNumeric(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    public static bool IsSTLOperation(this string input){
        return input.Equals(Tokens.MATCHES_STL);
    }

    public static bool IsVariable(this string input)
    {
        return !input.StartsWith('\"')
            && !input.EndsWith('\"')
            && !input.IsNumeric()
            && !input.IsPrimitive();
    }

    public static bool IsSliceIndexer(this string input, Queue<string> tokens)
    {
        return input.Equals(Tokens.SEMICOLON) && tokens.NextTokenIs(t => t.IsNumeric()) ||
               input.IsNumeric() && tokens.NextTokenIs(Tokens.SEMICOLON);
    }

    public static string RemoveCommentLines(this string input)
    {
        return string.Join(Environment.NewLine, input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
            .Where(line => !line.TrimStart()
            .StartsWith("//"))
        );
    }

    public static bool IsPrimitive(this string input)
    {
        return input.Equals(Tokens.BOOL_TRUE) 
            || input.Equals(Tokens.BOOL_FALSE) 
            || input.Equals(Tokens.NULL);
    }

    public static bool IsKeyword(this string input)
    {
        return Tokens
            .GetKeywords()
            .Contains(input);
    }
}