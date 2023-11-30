using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;

namespace NovaBASIC.Language.Parsing.Parsers;

[NodeParser(Tokens.LET)]
public class VariableDeclarationParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var variable = tokens.Dequeue();
        tokens.Dequeue(); //Pop the '='.

        return new VariableDeclarationNode(variable, parser.ParseNode());
    }
}
