using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class FieldAssignNode(AstNode term, string field, AstNode value) : AstNode
{
    public AstNode Term { get; } = term;
    public string Field { get; } = field;
    public AstNode Value { get; } = value;
}