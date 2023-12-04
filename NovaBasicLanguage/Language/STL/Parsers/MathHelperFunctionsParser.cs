using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Exceptions;
using NovaBasicLanguage.Language.Exceptions.Assertion;

namespace NovaBasic.Language.STL.Parsers;

[NodeParser("MATHHELPERS")]
public class MathHelperFunctionsParser : INodeParser
{
    public static string[] KNOWN_TOKENS =
    [
        Tokens.SIN_STL,
        Tokens.COS_STL,
        Tokens.TAN_STL,
        Tokens.FLOOR_STL,
        Tokens.CEIL_STL
    ];

    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        Assert.NextTokenIsCorrectThenDequeue(tokens, currentToken, Tokens.OPENING_PARENTHESIS);
        var operand = parser.ParseTernary();
        Assert.NextTokenIsCorrectThenDequeue(tokens, currentToken, Tokens.CLOSING_PARENTHESIS);
        return new TrigonometricNode(currentToken, operand);
    }
}
