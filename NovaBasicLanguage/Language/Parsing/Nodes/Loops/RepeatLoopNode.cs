using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Loops;

public class RepeatLoopNode(AstNode condition, AstNode[] body) : AstNode
{
    public AstNode Condition { get; set; } = condition;
    public AstNode[] Body { get; set; } = body;
}
