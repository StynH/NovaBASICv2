using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Extensions;
using NovaBASIC.Language.Exceptions;
using NovaBasicLanguage.Language.Parsing.Nodes.Loops;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_FOR)]
public class ForLoopParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var condition = parser.ParseTernary();
        if (!tokens.NextTokenIs(Tokens.KEYWORD_TO))
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FOR, Tokens.KEYWORD_TO);
        }
        tokens.Dequeue();
        var until = parser.ParseTernary();

        //Default stepsize is 1.
        AstNode stepSize = new ConstantNode<int>(1);
        if(tokens.NextTokenIs(Tokens.KEYWORD_STEP))
        {
            tokens.Dequeue();
            stepSize = parser.ParseTernary();
        }

        var terminatedSuccesfully = false;
        var body = new List<AstNode>();
        while (tokens.Count != 0)
        {
            if (tokens.NextTokenIs(Tokens.KEYWORD_END_FOR))
            {
                tokens.Dequeue();
                terminatedSuccesfully = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedSuccesfully)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FOR, Tokens.KEYWORD_END_FOR);
        }

        return new ForLoopNode(condition, until, stepSize, [..body]);
    }
}
