using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBasicLanguage.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_NOT)]
public class NotParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        return new NotNode(parser.ParseTernary());
    }
}
