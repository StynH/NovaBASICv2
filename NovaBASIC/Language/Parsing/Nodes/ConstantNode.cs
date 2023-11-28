namespace NovaBASIC.Language.Parsing.Nodes;

public class ConstantNode<T>(T value) : AstNode
{
    public T Value { get; private set; } = value;
}