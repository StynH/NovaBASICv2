namespace NovaBASIC.Language.Parsing.Nodes;

public class BinaryNode(AstNode left, string op, AstNode right) : AstNode
{
    public AstNode Left { get; set; } = left;
    public string Op { get; set; } = op;
    public AstNode Right { get; set; } = right;
}
