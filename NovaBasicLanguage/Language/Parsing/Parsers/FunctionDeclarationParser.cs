using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBasicLanguage.Extensions;
using NovaBasicLanguage.Language.Parsing.Nodes.Declarations;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_FUNC)]
public class FunctionDeclarationParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var name = tokens.Dequeue();
        if(tokens.Dequeue() != Tokens.OPENING_PARENTHESIS)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FUNC, Tokens.OPENING_PARENTHESIS);
        }

        //Parameters
        var terminatedCorrectly = false;
        var parameters = new List<string>();
        while(tokens.Count > 0)
        {
            if(tokens.NextTokenIs(Tokens.CLOSING_PARENTHESIS))
            {
                tokens.Dequeue();
                terminatedCorrectly = true;
                break;
            }

            parameters.Add(tokens.Dequeue());
            if(tokens.NextTokenIs(Tokens.COMMA))
            {
                tokens.Dequeue(); //Pop ','.
            }
        }

        if(!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FUNC, Tokens.CLOSING_PARENTHESIS);
        }

        //Body
        terminatedCorrectly = false;
        var body = new List<AstNode>();
        while(tokens.Count > 0)
        {
            if(tokens.NextTokenIs(Tokens.KEYWORD_END_FUNC))
            {
                tokens.Dequeue();
                terminatedCorrectly = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_FUNC, Tokens.KEYWORD_END_FUNC);
        }

        return new FunctionDeclarationNode(name, [..parameters], [..body]);
    }
}
