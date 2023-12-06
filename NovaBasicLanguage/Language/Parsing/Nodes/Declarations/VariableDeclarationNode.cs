using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes.Declarations;

public class VariableDeclarationNode(AstNode term, AstNode assignment, bool isImmutable = false) : AstNode
{
    public AstNode Term { get; set; } = term;
    public AstNode Assignment { get; set; } = assignment;
    public bool IsImmutable { get; set; } = isImmutable;
}
