namespace NovaBASIC.Language.STL.Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class StlFunctionAttribute(Type associatedNodeType) : System.Attribute
{
    public Type AssociatedNodeType { get; } = associatedNodeType;
}