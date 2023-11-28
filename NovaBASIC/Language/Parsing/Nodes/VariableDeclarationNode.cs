namespace NovaBASIC.Language.Parsing.Nodes;

public class VariableDeclarationNode(string name, AstNode assignment) : AstNode
{
    public string Name { get; set; } = name;
    public AstNode Assignment { get; set; } = assignment;
}
