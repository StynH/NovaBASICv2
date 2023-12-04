using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Loops;

public class ForLoopNode(AstNode condition, AstNode until, AstNode stepSize, AstNode[] body) : AstNode
{
    public AstNode Condition { get; set; } = condition;
    public AstNode Until { get; set; } = until;
    public AstNode StepSize { get; } = stepSize;
    public AstNode[] Body { get; set; } = body;
}
