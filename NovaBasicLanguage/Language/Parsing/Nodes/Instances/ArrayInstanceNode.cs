using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Instances;

public class ArrayInstanceNode(AstNode size, ArrayInstanceNode? sub = null) : AstNode
{
    public AstNode Size { get; set; } = size;
    public ArrayInstanceNode? Sub { get; set; } = sub;
}

