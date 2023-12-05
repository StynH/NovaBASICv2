using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Extensions;
using NovaBASIC.Extensions;
using NovaBasicLanguage.Language.Parsing.Nodes.Declarations;
using NovaBasicLanguage.Language.Parsing.Nodes.Instances;

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

        //Struct initializing
        if(tokens.NextTokenIs(token => token.IsVariable()))
        {
            return new NewInstanceNode(ParseStruct(tokens, parser));
        }

        return new NewInstanceNode(parser.ParseTerm());
    }

    private static AstNode ParseArray(Queue<string> tokens, Parser parser)
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
            return new ArrayInstanceNode(size, ParseArray(tokens, parser) as ArrayInstanceNode);
        }
        else
        {
            return new ArrayInstanceNode(size);
        }
    }

    private AstNode ParseStruct(Queue<string> tokens, Parser parser)
    {
        var structName = tokens.Dequeue();
        var parameters = new List<AstNode>();
        if(tokens.NextTokenIs(Tokens.OPENING_PARENTHESIS))
        {
            tokens.Dequeue(); //Pop '('.
            while (!tokens.NextTokenIs(Tokens.CLOSING_PARENTHESIS))
            {
                parameters.Add(parser.ParseTernary());
                if(tokens.NextTokenIs(Tokens.COMMA))
                {
                    tokens.Dequeue();
                }
            }
            tokens.Dequeue(); //Pop ')'.
        }

        return new StructInstanceNode(structName, [.. parameters]);
    }
}
