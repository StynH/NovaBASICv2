using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Extensions;
using NovaBASIC.Language.Exceptions;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.STL.Nodes;
using NovaBasicLanguage.Language.Exceptions.Assertion;
namespace NovaBasicLanguage.Language.STL.Parsers;

[NodeParser(Tokens.RAND_STL)]
public class RandomParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        Assert.NextTokenIsCorrectThenDequeue(tokens, currentToken, Tokens.OPENING_PARENTHESIS);

        AstNode min = new ConstantNode<float>(0f);
        AstNode max = new ConstantNode<float>(1f);
        bool hasParameters = false;
        if (!tokens.NextTokenIs(Tokens.CLOSING_PARENTHESIS))
        {
            min = parser.ParseTernary();
            if (!tokens.NextTokenIs(Tokens.COMMA))
            {
                throw new MissingParameterException(Tokens.RAND_STL, "max");
            }
            tokens.Dequeue();
            max = parser.ParseTernary();
            hasParameters = true;
        }

        if (hasParameters)
        {
            Assert.NextTokenIsCorrectThenDequeue(tokens, currentToken, Tokens.CLOSING_PARENTHESIS);
        }
        else
        {
            tokens.Dequeue();
        }

        return new RandomNode(min, max);
    }
}
