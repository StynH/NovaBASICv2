using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasic.Language.STL.Nodes;

public class PrintNode(AstNode message) : AstNode
{
    public AstNode Message { get; set; } = message;
}
