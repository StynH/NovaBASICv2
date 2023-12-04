using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Language.STL.Nodes;
using NovaBasicLanguage.Language.Exceptions.Assertion;

namespace NovaBasicLanguage.Language.STL.Parsers;

[NodeParser(Tokens.COUNT_STL)]
public class CountParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        Assert.NextTokenIsCorrectThenDequeue(tokens, currentToken, Tokens.OPENING_PARENTHESIS);
        var operand = parser.ParseTernary();
        Assert.NextTokenIsCorrectThenDequeue(tokens, currentToken, Tokens.CLOSING_PARENTHESIS);
        return new CountNode(operand);
    }
}
