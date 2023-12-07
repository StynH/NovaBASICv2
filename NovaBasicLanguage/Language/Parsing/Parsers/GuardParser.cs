using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Extensions;
using NovaBasicLanguage.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_GUARD)]
public class GuardParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var condition = parser.ParseTernary();
        if (!tokens.NextTokenIs(Tokens.KEYWORD_ELSE))
        {
            throw new MalformedStatementException(Tokens.KEYWORD_GUARD, Tokens.KEYWORD_ELSE);
        }

        tokens.Dequeue(); //Pop 'ELSE'.

        var terminatedCorrectly = false;
        var body = new List<AstNode>();
        while (tokens.Count > 0)
        {
            if (tokens.NextTokenIs(Tokens.KEYWORD_END_GUARD))
            {
                tokens.Dequeue(); //Pop 'ENDGUARD'.
                terminatedCorrectly = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_GUARD, Tokens.KEYWORD_END_GUARD);
        }

        return new GuardNode(condition, body);
    }
}