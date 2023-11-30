using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Runtime;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Runtime;
using System.Reflection;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    private RuntimeContext _runtimeContext = new();

    private bool _returnIsCalled;

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

    public void Visit<T>(ConstantNode<T> node)
    {
        Result = node.Value;
    }

    public void Visit(VariableNode node)
    {
        //TODO: The '.Value' thing should be different.
        Result = _runtimeContext.Get(node.Name).Value;
    }

    public void Visit(VariableDeclarationNode node)
    {
        var name = node.Name;
        var value = ExecuteNode(node.Assignment);

        if(value is MemoryReference reference)
        {
            _runtimeContext.AssignReference(name, reference);
            Result = null;
            return;
        }

        _runtimeContext.Assign(name, value, false);
        Result = null;
    }

    public void Visit(ReferenceNode node)
    {
        Result = new MemoryReference(node.VariableName);
    }

    public void Visit(ConditionalNode node)
    {
        var conditionResult = (bool)ExecuteNode(node.Condition)!;
        if (conditionResult)
        {
            CreateScope();
            foreach (var expr in node.TrueBody) { 
                ExecuteNode(expr);
                if (_returnIsCalled)
                {
                    _returnIsCalled = false;
                    break;
                }
            }
            PopScope();
        }
        else
        {
            node.Else?.Accept(this);
        }
        Result = null;
    }

    public object? ExecuteNode(AstNode node)
    {
        node.Accept(this);
        return Result;
    }

    public RuntimeContext GetRuntimeContext()
    {
        return _runtimeContext;
    }

    private void CreateScope()
    {
        _runtimeContext = _runtimeContext.CreateChildRuntimeContext();
    }

    private void PopScope()
    {
        _runtimeContext = _runtimeContext.PopRuntimeContext() ?? _runtimeContext;
    }
}
