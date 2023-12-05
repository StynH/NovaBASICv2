using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class FieldAccessorNode(string variable, string name) : AstNode
{
    public string Variable { get; } = variable;
    public string Name { get; } = name;
}
