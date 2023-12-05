using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Declarations;

public class StructDeclarationNode(string name, string[] fields) : AstNode
{
    public string Name { get; set; } = name;
    public string[] Fields { get; set; } = fields;
}
