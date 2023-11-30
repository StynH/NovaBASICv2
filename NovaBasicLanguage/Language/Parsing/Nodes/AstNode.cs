using NovaBASIC.Language.Interpreting.Interface;

namespace NovaBASIC.Language.Parsing.Nodes;

public abstract class AstNode
{
    protected AstNode()
    {}

    public void Accept(INodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}
