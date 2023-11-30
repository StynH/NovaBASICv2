namespace NovaBASIC.Language.STL.Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class StlFunctionAttribute(string functionName) : System.Attribute
{
    public string FunctionName { get; private set; } = functionName;
}