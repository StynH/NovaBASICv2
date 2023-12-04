using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Extensions;
using NovaBasicLanguage.Language.Parsing.Nodes.Loops;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_WHILE)]
public class WhileLoopParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var condition = parser.ParseTernary();
        var terminatedSuccesfully = false;
        var body = new List<AstNode>();
        while (tokens.Count != 0)
        {
            if (tokens.NextTokenIs(Tokens.KEYWORD_END_WHILE))
            {
                tokens.Dequeue();
                terminatedSuccesfully = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedSuccesfully)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_WHILE, Tokens.KEYWORD_END_WHILE);
        }

        return new WhileLoopNode(condition, [.. body]);
    }
}

