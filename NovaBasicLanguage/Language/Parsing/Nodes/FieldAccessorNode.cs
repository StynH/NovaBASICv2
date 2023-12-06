using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class FieldAccessorNode(AstNode term, string name) : AstNode
{
    public AstNode Term { get; } = term;
    public string Name { get; } = name;
}
