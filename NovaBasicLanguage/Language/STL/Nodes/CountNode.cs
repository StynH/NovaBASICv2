using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.STL.Nodes;

public class CountNode(AstNode operand) : AstNode
{
    public AstNode Operand { get; set; } = operand;
}

