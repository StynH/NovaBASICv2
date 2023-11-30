namespace NovaBASIC.Language.Parsing.Nodes;

public class ConditionalNode(AstNode condition, IList<AstNode> trueBody, AstNode? elseNode) : AstNode
{
    public AstNode Condition { get; set; } = condition;
    public IList<AstNode> TrueBody { get; set; } = trueBody;
    public AstNode? Else { get; set; } = elseNode;
}
