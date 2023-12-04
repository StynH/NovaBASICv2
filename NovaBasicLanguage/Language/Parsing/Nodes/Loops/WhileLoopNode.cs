using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Loops;

public class WhileLoopNode(AstNode condition,AstNode[] body) : AstNode
{
    public AstNode Condition { get; set; } = condition;
    public AstNode[] Body { get; set; } = body;
}

