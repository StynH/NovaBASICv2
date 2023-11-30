namespace NovaBASIC.Language.Parsing.Nodes;

public class VariableDeclarationNode(string name, AstNode assignment, bool isImmutable = false) : AstNode
{
    public string Name { get; set; } = name;
    public AstNode Assignment { get; set; } = assignment;
    public bool IsImmutable { get; set; } = isImmutable;
}
