using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Instances;

public class StructInstanceNode(string name, AstNode[] parameters) : AstNode
{
    public string Name { get; } = name;
    public AstNode[] Parameters { get; } = parameters;
}
