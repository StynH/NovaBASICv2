using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBASIC.Extensions;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_RETURN)]
public class ReturnParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        if (tokens.TryPeek(out var next) 
            && !next.Equals(Tokens.KEYWORD_NEW) 
            && next.IsKeyword())
        {
            return new ReturnNode(new NullNode());
        }
        return new ReturnNode(parser.ParseTernary());
    }
}