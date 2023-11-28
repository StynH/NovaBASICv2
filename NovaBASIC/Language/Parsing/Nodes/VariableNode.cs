namespace NovaBASIC.Language.Parsing.Nodes;

public class VariableNode(string name): AstNode
{
    public string Name { get; set; } = name;
}
