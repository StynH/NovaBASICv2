using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Language.STL.Nodes;

namespace NovaBasicLanguage.Language.STL.Parsers;

[NodeParser(Tokens.COUNT_STL)]
public class CountParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        return new CountNode(parser.ParseTernary());
    }
}
