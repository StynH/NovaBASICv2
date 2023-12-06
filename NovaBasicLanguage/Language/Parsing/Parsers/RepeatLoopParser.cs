using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBasicLanguage.Extensions;
using NovaBasicLanguage.Language.Parsing.Nodes.Loops;
using NovaBASIC.Language.Parsing.Parsers.Interface;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_REPEAT)]
public class RepeatLoopParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var terminatedSuccesfully = false;
        var body = new List<AstNode>();
        while (tokens.Count != 0)
        {
            if (tokens.NextTokenIs(Tokens.KEYWORD_UNTIL))
            {
                tokens.Dequeue();
                terminatedSuccesfully = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedSuccesfully)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_REPEAT, Tokens.KEYWORD_UNTIL);
        }

        var condition = parser.ParseTernary();

        return new RepeatLoopNode(condition, [.. body]);
    }
}
