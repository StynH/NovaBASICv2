using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class TypeNode(string typeName) : AstNode
{
    public string TypeName { get; } = typeName;
}
