using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class GroupNode(AstNode inner) : AstNode
{
    public AstNode Inner { get; } = inner;
}
