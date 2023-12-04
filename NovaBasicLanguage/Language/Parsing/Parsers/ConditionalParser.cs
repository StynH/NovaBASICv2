using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBasicLanguage.Extensions;

namespace NovaBASIC.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_IF)]
public class ConditionalParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        return ParseConditional(tokens, currentToken, parser);
    }

    private static ConditionalNode ParseConditional(Queue<string> tokens, string currentToken, Parser parser)
    {
        var condition = currentToken != Tokens.KEYWORD_ELSE ? parser.ParseTernary() : new ConstantNode<bool>(true);
        if (!tokens.NextTokenIs(Tokens.KEYWORD_THEN))
        {
            throw new MalformedStatementException(Tokens.KEYWORD_IF, Tokens.KEYWORD_THEN);
        }

        tokens.Dequeue(); //Pop 'THEN'.

        var terminatedCorrectly = false;
        var body = new List<AstNode>();
        AstNode? elseNode = null;
        while (tokens.Count > 0)
        {
            if(tokens.NextTokenIs(Tokens.KEYWORD_ELSE) || tokens.NextTokenIs(Tokens.KEYWORD_ELSEIF))
            {
                var token = tokens.Dequeue();
                elseNode = ParseConditional(tokens, token, parser);
            }

            if(tokens.NextTokenIs(Tokens.KEYWORD_ENDIF) && (currentToken == Tokens.KEYWORD_ELSE || currentToken == Tokens.KEYWORD_ELSEIF))
            {
                return new ConditionalNode(condition, body, elseNode);
            }
            else if(tokens.NextTokenIs(Tokens.KEYWORD_ENDIF))
            {
                tokens.Dequeue(); //Pop 'ENDIF'.
                terminatedCorrectly = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedCorrectly)
        {
            throw new MalformedStatementException(currentToken, Tokens.KEYWORD_ENDIF);
        }

        return new ConditionalNode(condition, body, elseNode);
    }
} 