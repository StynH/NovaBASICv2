using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Array;

public class ArrayIndexingNode(AstNode operand, AstNode index, ArrayIndexingNode? sub = null) : AstNode
{
    public AstNode Operand { get; set; } = operand;
    public AstNode Index { get; set; } = index;
    public ArrayIndexingNode? Sub { get; set; } = sub;
}