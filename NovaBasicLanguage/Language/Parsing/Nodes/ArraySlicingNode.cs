using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class ArraySlicingNode(AstNode begin, AstNode end, AstNode step) : AstNode
{
    public AstNode Begin { get; } = begin;
    public AstNode End { get; } = end;
    public AstNode Step { get; } = step;
}
