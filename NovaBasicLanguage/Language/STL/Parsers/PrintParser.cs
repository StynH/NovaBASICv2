using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Lexicon;

namespace NovaBasic.Language.STL.Parsers;

[NodeParser(Tokens.PRINT_STL)]
public class PrintParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        return new PrintNode(parser.ParseTernary());
    }
}
