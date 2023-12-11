using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

public class InstanceOfNode(AstNode operand, string typeName) : AstNode
{
    public AstNode Operand { get; } = operand;
    public string TypeName { get; } = typeName;
}
