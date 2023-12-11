﻿using System.Reflection;
using System.Text.RegularExpressions;

namespace NovaBASIC.Language.Lexicon;

public static partial class Tokens
{
    public const string NEGATIVE_NUMERALS_PATTERN = "-\\d+(\\.\\d+)?";

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
    public const string BITWISE_LEFT_SHIFT = "<<";
    public const string BITWISE_RIGHT_SHIFT = ">>";
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
    public const string ACCESSOR = ".";
    public const string BITWISE_AND = "&";
    public const string BITWISE_OR = "|";
    public const string BITWISE_XOR = "^";
    public const string BITWISE_NOT = "~";

    // Syntax
    public const string KEYWORD_NEW = "NEW";
    public const string KEYWORD_IF = "IF";
    public const string KEYWORD_THEN = "THEN";
    public const string KEYWORD_RETURN = "RETURN";
    public const string KEYWORD_ENDIF = "ENDIF";
    public const string KEYWORD_FUNC = "FUNC";
    public const string KEYWORD_END_FUNC = "ENDFUNC";
    public const string KEYWORD_GUARD = "GUARD";
    public const string KEYWORD_END_GUARD = "ENDGUARD";
    public const string KEYWORD_ELSEIF = "ELSEIF";
    public const string KEYWORD_ELSE = "ELSE";
    public const string KEYWORD_REF = "REF";
    public const string KEYWORD_STRUCT = "STRUCT";
    public const string KEYWORD_END_STRUCT = "ENDSTRUCT";

    public const string DECLARATION_PATTERN = "[a-zA-Z_$][a-zA-Z0-9_$]*";
    public const string OPENING_PARENTHESIS = "(";
    public const string CLOSING_PARENTHESIS = ")";

    public const string KEYWORD_LET = "LET";
    public const string KEYWORD_IMMUTABLE = "IMMUTABLE";
    public const string KEYWORD_WHILE = "WHILE";
    public const string KEYWORD_END_WHILE = "ENDWHILE";
    public const string KEYWORD_FOR = "FOR";
    public const string KEYWORD_END_FOR = "ENDFOR";
    public const string KEYWORD_REPEAT = "REPEAT";
    public const string KEYWORD_UNTIL = "UNTIL";
    public const string KEYWORD_TO = "TO";
    public const string KEYWORD_STEP = "STEP";
    public const string KEYWORD_BREAK = "BREAK";
    public const string KEYWORD_NOT = "NOT";
    public const string KEYWORD_SWITCH = "SWITCH";
    public const string KEYWORD_END_SWITCH = "ENDSWITCH";
    public const string KEYWORD_CASE = "CASE";
    public const string KEYWORD_DEFAULT = "DEFAULT";
    public const string KEYWORD_IS = "IS";

    public const string TYPE_STRING = "STRING";
    public const string TYPE_INT = "INT";
    public const string TYPE_FLOAT = "FLOAT";
    public const string TYPE_ARRAY = "ARRAY";

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

    public static IList<string> GetKeywordsAndStlFunctions()
    {
        var fields = typeof(Tokens)
             .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
             .Where(field => field.FieldType == typeof(string))
             .Select(field => new
             {
                 field.Name,
                 Value = field.GetValue(null) as string
             });

        return fields
            .Where(f => f.Name.StartsWith("KEYWORD_") || f.Name.EndsWith("_STL"))
            .Select(f => f.Value!)
            .ToList();
    }

    public static IList<string> GetKeywords()
    {
        var fields = typeof(Tokens)
             .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
             .Where(field => field.FieldType == typeof(string))
             .Select(field => new
             {
                 field.Name,
                 Value = field.GetValue(null) as string
             });

        return fields
            .Where(f => f.Name.StartsWith("KEYWORD_"))
            .Select(f => f.Value!)
            .ToList();
    }

    public static IList<string> GetTypes()
    {
        var fields = typeof(Tokens)
             .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
             .Where(field => field.FieldType == typeof(string))
             .Select(field => new
             {
                 field.Name,
                 Value = field.GetValue(null) as string
             });

        return fields
            .Where(f => f.Name.StartsWith("TYPE_"))
            .Select(f => f.Value!)
            .ToList();
    }
}
