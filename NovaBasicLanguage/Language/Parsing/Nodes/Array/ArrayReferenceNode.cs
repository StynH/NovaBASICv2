using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Array;

public class ArrayReferenceNode(string variableName, ArrayIndexingNode index) : AstNode
{
    public string VariableName { get; set; } = variableName;
    public ArrayIndexingNode Index { get; } = index;
}
