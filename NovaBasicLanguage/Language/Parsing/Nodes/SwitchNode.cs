using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class SwitchNode(AstNode operand, CaseNode[] caseStatements, CaseNode? defaultStatement) : AstNode
{
    public AstNode Operand { get; } = operand;
    public CaseNode[] CaseStatements { get; } = caseStatements;
    public CaseNode? DefaultStatement { get; } = defaultStatement;
}
