using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class NewInstanceNode(AstNode operand) : AstNode
{
    public AstNode Operand { get; set; } = operand;
}

