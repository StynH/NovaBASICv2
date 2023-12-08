using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Extensions;
using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Extensions;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser("SLICING_INDEX")]
public class ArraySlicingParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var begin = 0;
        var end = int.MaxValue;
        var step = 1;

        if (currentToken.IsNumeric())
        {
            begin = int.Parse(currentToken);
        }
        
        if(tokens.NextTokenIs(Tokens.SEMICOLON))
        {
            tokens.Dequeue();
        }

        if(tokens.TryPeek(out var next) && next.IsNumeric())
        {
            end = int.Parse(next);
            tokens.Dequeue();
        }

        if (tokens.NextTokenIs(Tokens.SEMICOLON))
        {
            tokens.Dequeue();
            if(tokens.NextTokenIs(t => t.IsNumeric()) && tokens.TryDequeue(out var stepSize))
            {
                step = int.Parse(stepSize);
            }
        }

        return new ArraySlicingNode(
            new ConstantNode<int>(begin),
            new ConstantNode<int>(end),
            new ConstantNode<int>(step)
        );
    }
}
