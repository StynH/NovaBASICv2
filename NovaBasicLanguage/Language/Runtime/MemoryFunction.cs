using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Runtime;

public class MemoryFunction(string name, string[] parameters, AstNode[] body)
{
    public string Name { get; set; } = name;
    public string[] Parameters { get; set; } = parameters;
    public AstNode[] Body { get; set; } = body;
}
