using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Extensions;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.REF)]
public class ReferenceParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        if(tokens.TryPeek(out var token) && !token.IsVariable()) {
            throw new WrongUsageException(Tokens.REF, "variables");
        }

        return new ReferenceNode(token!);
    }
}
