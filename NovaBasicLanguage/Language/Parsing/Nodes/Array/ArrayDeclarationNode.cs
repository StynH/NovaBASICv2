using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Array;

public class ArrayDeclarationNode(AstNode size, ArrayDeclarationNode? sub = null) : AstNode
{
    public AstNode Size { get; set; } = size;
    public ArrayDeclarationNode? Sub { get; set; } = sub;
}

