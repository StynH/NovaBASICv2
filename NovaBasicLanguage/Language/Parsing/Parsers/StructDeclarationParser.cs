using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Language.Parsing.Nodes.Declarations;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Exceptions;
using NovaBasicLanguage.Extensions;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_STRUCT)]
public class StructDeclarationParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var name = tokens.Dequeue();
        var fields = new List<string>();

        var terminatedSuccesfully = false;
        while (tokens.Count > 0)
        {
            fields.Add(tokens.Dequeue());
            if(tokens.NextTokenIs(Tokens.COMMA))
            {
                tokens.Dequeue(); //Pop ','.
            }
            else if(tokens.NextTokenIs(Tokens.KEYWORD_END_STRUCT))
            {
                tokens.Dequeue(); //Pop 'ENDSTRUCT'.
                terminatedSuccesfully = true;
                break;
            }
        }

        if (!terminatedSuccesfully)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_STRUCT, Tokens.KEYWORD_END_STRUCT);
        }

        return new StructDeclarationNode(name, [.. fields]);
    }
}
