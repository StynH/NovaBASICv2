using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBASIC.Language.Interpreting.Interface;

public interface INodeVisitor
{
    void Visit<T>(T element) where T : AstNode;
}
