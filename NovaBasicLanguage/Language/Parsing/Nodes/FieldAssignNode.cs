using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class FieldAssignNode(string variable, string field, AstNode value) : AstNode
{
    public string Variable { get; } = variable;
    public string Field { get; } = field;
    public AstNode Value { get; } = value;
}