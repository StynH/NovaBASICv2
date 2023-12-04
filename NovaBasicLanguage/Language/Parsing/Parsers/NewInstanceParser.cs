using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;
using NovaBasicLanguage.Extensions;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_NEW)]
public class NewInstanceParser : INodeParser
{
    private static readonly int DEFAULT_ARRAY_SIZE = 8;

    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        //Array initializing
        if(tokens.NextTokenIs(Tokens.OPENING_BRACKET))
        {
            return new NewInstanceNode(ParseArray(tokens, parser));
        }

        return new NewInstanceNode(parser.ParseTerm());
    }

    public static AstNode ParseArray(Queue<string> tokens, Parser parser)
    {
        tokens.Dequeue(); //Pop '['.

        AstNode size = new ConstantNode<int>(DEFAULT_ARRAY_SIZE);
        if (!tokens.NextTokenIs(Tokens.CLOSING_BRACKET))
        {
            size = parser.ParseTernary();
        }

        tokens.Dequeue(); //Pop ']'.

        if(tokens.NextTokenIs(Tokens.OPENING_BRACKET))
        {
            return new ArrayDeclarationNode(size, ParseArray(tokens, parser) as ArrayDeclarationNode);
        }
        else
        {
            return new ArrayDeclarationNode(size);
        }
    }
}
