using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Language.Exceptions.Assertion;
using NovaBasicLanguage.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.OPENING_PARENTHESIS)]
public class GroupParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var inner = parser.ParseTernary();
        Assert.NextTokenIsCorrectThenDequeue(tokens, Tokens.OPENING_PARENTHESIS, Tokens.CLOSING_PARENTHESIS);
        return new GroupNode(inner);
    }
}
