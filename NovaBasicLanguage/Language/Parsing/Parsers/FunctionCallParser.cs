using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Extensions;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser("FUNC_CALL")]
public class FunctionCallParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        if (tokens.Dequeue() != Tokens.OPENING_PARENTHESIS) //Try pop '('.
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FUNC, Tokens.OPENING_PARENTHESIS);
        }

        //Parameters
        var terminatedCorrectly = false;
        var parameters = new List<AstNode>();
        while (tokens.Count > 0)
        {
            if (tokens.NextTokenIs(Tokens.CLOSING_PARENTHESIS))
            {
                tokens.Dequeue();
                terminatedCorrectly = true;
                break;
            }

            parameters.Add(parser.ParseTernary());
            if (tokens.NextTokenIs(Tokens.COMMA))
            {
                tokens.Dequeue(); //Pop ','.
            }
        }

        if (!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FUNC, Tokens.CLOSING_PARENTHESIS);
        }

        return new FunctionCallNode(currentToken, [.. parameters]);
    }
}