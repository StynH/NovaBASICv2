using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Exceptions;

namespace NovaBasic.Language.STL.Parsers;

[NodeParser("TRIGONOMETRIC")]
public class TrigonometricParser : INodeParser
{
    public static string[] KNOWN_TOKENS =
    [
        Tokens.SIN_STL,
        Tokens.COS_STL,
        Tokens.TAN_STL
    ];

    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        if(tokens.Dequeue() != Tokens.OPENING_PARENTHESIS)
        {
            throw new MalformedStatementException(currentToken, Tokens.OPENING_PARENTHESIS);
        }

        var operand = parser.ParseNode();

        if (tokens.Dequeue() != Tokens.CLOSING_PARENTHESIS)
        {
            throw new MalformedStatementException(currentToken, Tokens.CLOSING_PARENTHESIS);
        }

        return new TrigonometricNode(currentToken, operand);
    }
}
