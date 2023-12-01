using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Array;

public class ArrayAssignNode(ArrayIndexingNode index, AstNode value) : AstNode
{
    public ArrayIndexingNode Index { get; set; } = index;
    public AstNode Value { get; set; } = value;
}