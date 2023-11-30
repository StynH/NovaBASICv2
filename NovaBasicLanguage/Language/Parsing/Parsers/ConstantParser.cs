using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;

namespace NovaBASIC.Language.Parsing.Parsers;

[NodeParser("CONSTANTS")]
public class ConstantParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        if (int.TryParse(currentToken, out int intVal))
        {
            return new ConstantNode<int>(intVal);
        }
        else if (float.TryParse(currentToken, out float floatVal))
        {
            return new ConstantNode<float>(floatVal);
        }
        else if (bool.TryParse(currentToken, out bool boolVal))
        {
            return new ConstantNode<bool>(boolVal);
        }
        else
        {
            return new ConstantNode<string>(currentToken[1..^1]);
        }
    }
}
