using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.STL.Nodes;

public class RandomNode(AstNode min, AstNode max) : AstNode
{
    public AstNode Min { get; } = min;
    public AstNode Max { get; } = max;
}
