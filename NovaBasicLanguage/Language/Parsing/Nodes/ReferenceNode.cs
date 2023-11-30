using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class ReferenceNode(string VariableName) : AstNode
{
    public string VariableName { get; set; } = VariableName;
}
