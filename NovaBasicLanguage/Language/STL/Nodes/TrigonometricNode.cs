using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasic.Language.STL.Nodes;

public class TrigonometricNode(string funcName, AstNode operand): AstNode
{
    public string FuncName { get; set; } = funcName;
    public AstNode Operand { get; set; } = operand;
}
