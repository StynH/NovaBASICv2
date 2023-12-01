using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class FunctionDeclarationNode(string name, string[] parameters, AstNode[] body) : AstNode
{
    public string Name { get; set; } = name;
    public string[] Parameters { get; set; } = parameters;
    public AstNode[] Body { get; set; } = body;
}
