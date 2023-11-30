using NovaBASIC.Language.Exceptions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;

namespace NovaBASIC.Language.Parsing.Parsers;

[NodeParser(Tokens.IF)]
public class ConditionalParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        return ParseConditional(tokens, currentToken, parser);
    }

    private static ConditionalNode ParseConditional(Queue<string> tokens, string currentToken, Parser parser)
    {
        var condition = currentToken != Tokens.ELSE ? parser.ParseTernary() : new ConstantNode<bool>(true);
        if (tokens.Peek() != Tokens.THEN)
        {
            throw new MalformedStatementException(Tokens.IF, Tokens.THEN);
        }

        tokens.Dequeue(); //Pop 'THEN'.

        var terminatedCorrectly = false;
        var body = new List<AstNode>();
        AstNode? elseNode = null;
        while (tokens.Count > 0)
        {
            if(tokens.Peek() == Tokens.ELSE || tokens.Peek() == Tokens.ELSEIF)
            {
                var token = tokens.Dequeue();
                elseNode = ParseConditional(tokens, token, parser);
            }

            if(tokens.Peek() == Tokens.ENDIF && (currentToken == Tokens.ELSE || currentToken == Tokens.ELSEIF))
            {
                return new ConditionalNode(condition, body, elseNode);
            }
            else if(tokens.Peek() == Tokens.ENDIF)
            {
                tokens.Dequeue(); //Pop 'ENDIF'.
                terminatedCorrectly = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedCorrectly)
        {
            throw new MalformedStatementException(currentToken, Tokens.ENDIF);
        }

        return new ConditionalNode(condition, body, elseNode);
    }
} 