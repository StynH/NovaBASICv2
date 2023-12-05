using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.References;

public class FieldReferenceNode(string variableName, string field) : AstNode
{
    public string VariableName { get; set; } = variableName;
    public string Field { get; } = field;
}