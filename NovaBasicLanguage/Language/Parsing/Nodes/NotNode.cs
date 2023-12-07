using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class NotNode(AstNode condition) : AstNode
{
    public AstNode Condition { get; set; } = condition;
}
