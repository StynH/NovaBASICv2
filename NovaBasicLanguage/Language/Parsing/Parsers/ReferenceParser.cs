using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Extensions;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;
using NovaBasicLanguage.Extensions;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_REF)]
public class ReferenceParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        if(tokens.TryDequeue(out var token)) {
            switch (token)
            {
                case var _ when !token.IsVariable():
                    throw new WrongUsageException(Tokens.KEYWORD_REF, "variables");
            }
        }

        if (tokens.TryPeek(out var next))
        {
            switch (next)
            {
                case Tokens.OPENING_BRACKET:
                    return ParseArrayReference(tokens, token!, parser);
            }
        }

        return new ReferenceNode(token!);
    }

    private AstNode ParseArrayReference(Queue<string> tokens, string token, Parser parser)
    {
        var variableNode = new VariableNode(token);
        var indexer = ParseIndexer(tokens, variableNode, parser);
        return new ArrayReferenceNode(token, indexer);
    }

    private ArrayIndexingNode ParseIndexer(Queue<string> tokens, VariableNode variable, Parser parser)
    {
        tokens.Dequeue(); //Pop '['.
        var index = parser.ParseTernary();
        tokens.Dequeue(); //Pop ']'.

        if(tokens.NextTokenIs(Tokens.OPENING_BRACKET))
        {
            return new ArrayIndexingNode(variable, index, ParseIndexer(tokens, variable, parser));
        }

        return new ArrayIndexingNode(variable, index);
    } 
}
