using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Language.STL.Nodes;
using NovaBasicLanguage.Language.Exceptions.Assertion;

namespace NovaBasicLanguage.Language.STL.Parsers;

[NodeParser(Tokens.TO_ARRAY_STL)]
public class ToArrayParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        Assert.NextTokenIsCorrectThenDequeue(tokens, Tokens.ARRAY_RESIZE_STL, Tokens.OPENING_PARENTHESIS);
        var value = parser.ParseTernary();
        Assert.NextTokenIsCorrectThenDequeue(tokens, Tokens.ARRAY_RESIZE_STL, Tokens.CLOSING_PARENTHESIS);
        return new ToArrayNode(value);
    }
}