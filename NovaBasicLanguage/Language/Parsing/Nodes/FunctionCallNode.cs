using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class FunctionCallNode(string name, AstNode[] parameters) : AstNode
{
    public string Name { get; set; } = name;
    public AstNode[] Parameters { get; set; } = parameters;
}
