using NovaBASIC.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;

namespace NovaBasicLanguage.Language.Parsing.Nodes.References;

public class ArrayReferenceNode(string variableName, ArrayIndexingNode index) : AstNode
{
    public string VariableName { get; set; } = variableName;
    public ArrayIndexingNode Index { get; } = index;
}
