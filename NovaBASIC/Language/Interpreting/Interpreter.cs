using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL;
using System.Reflection;

namespace NovaBASIC.Language.Interpreting;

public class Interpreter : INodeVisitor
{
    private StandardLibrary _stl = new StandardLibrary();

    public object? Result { get; set; } = null;

    public void RunProgram(IList<AstNode> nodes)
    {
        foreach(var node in nodes)
        {
            ExecuteNode(node);
        }
    }

    public void Visit<T>(T node) where T : AstNode
    {
        var method = ResolveVisitMethod(node.GetType());

        if (method != null)
        {
            if (node.GetType().GetGenericArguments().Length > 0)
            {
                method = method.MakeGenericMethod([node.GetType().GetGenericArguments()[0]]);
            }
            method.Invoke(this, new object[] { node });
        }
        else
        {
            throw new MissingMethodException("No Visit method found for type " + node.GetType());
        }
    }

    private MethodInfo? ResolveVisitMethod(Type nodeType)
    {
        var methods = GetType().GetMethods()
            .Where(m => m.Name == "Visit" && m.GetParameters().Length == 1);

        foreach (var method in methods)
        {
            var paramType = method.GetParameters().First().ParameterType;

            if (paramType == nodeType)
            {
                return method;
            }
            
            if (paramType.IsGenericType && nodeType.IsGenericType &&
                paramType.GetGenericTypeDefinition() == typeof(ConstantNode<>) &&
                nodeType.GetGenericTypeDefinition() == typeof(ConstantNode<>))
            {
                return method;
            }
        }

        return null;
    }

    public void Visit(BinaryNode node)
    {
        _stl
            .GetFunction("MATH")
            .Execute(this, node);
    }

    public void Visit<T>(ConstantNode<T> node)
    {
        Result = node.Value;
    }

    public object? ExecuteNode(AstNode node)
    {
        node.Accept(this);
        return Result;
    }
}
