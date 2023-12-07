using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Runtime;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.Helpers;
using NovaBasicLanguage.Language.Interpreting;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;
using NovaBasicLanguage.Language.Parsing.Nodes.Declarations;
using NovaBasicLanguage.Language.Parsing.Nodes.Instances;
using NovaBasicLanguage.Language.Parsing.Nodes.Loops;
using NovaBasicLanguage.Language.Parsing.Nodes.References;
using NovaBasicLanguage.Language.Runtime;
using System.Reflection;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    private RuntimeContext _runtimeContext = new(true);

    private Dictionary<Type, MethodInfo> _methodsCache = [];

    private bool _returnIsCalled;
    private bool _breakIsCalled;

    public InterpretResult Result { get; set; } = new InterpretResult();

    public void RunProgram(IList<AstNode> nodes)
    {
        foreach (var node in nodes)
        {
            ExecuteNode(node);
        }
    }

    public void Visit<T>(T node) where T : AstNode
    {
        Type nodeType = node.GetType();
        MethodInfo? method;

        if (!_methodsCache.TryGetValue(nodeType, out method))
        {
            method = ResolveVisitMethod(nodeType);
            if (method != null)
            {
                if (nodeType.GetGenericArguments().Length > 0)
                {
                    method = method.MakeGenericMethod(nodeType.GetGenericArguments()[0]);
                }
                _methodsCache[nodeType] = method;
            }
            else
            {
                VisitStl(node);
                return;
            }
        }

        method.Invoke(this, new object[] { node });
    }

    public void InitializeMethodCache()
    {
        var visitMethods = GetType()
                           .GetMethods()
                           .Where(m => m.Name == "Visit" && m.GetParameters().Length == 1
                                           && typeof(AstNode).IsAssignableFrom(m.GetParameters()[0].ParameterType));

        foreach (var method in visitMethods)
        {
            var paramType = method.GetParameters()[0].ParameterType;
            MethodInfo methodToCache;

            if (method.IsGenericMethod)
            {
                methodToCache = method.GetGenericMethodDefinition();
            }
            else
            {
                methodToCache = method;
            }

            _methodsCache[paramType] = methodToCache;
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
        Result.Set(node.Value);
    }

    public void Visit(NullNode _)
    {
        Result.ToNull();
    }

    public void Visit(VariableNode node)
    {
        var variable = _runtimeContext.GetVariable(node.Name);
        Result.Set(variable.Value, variable);
    }

    public void Visit(VariableDeclarationNode node)
    {
        var result = ExecuteNode(node.Term);
        var variable = result.Operand ?? result.GetStoredItem();
        var value = ExecuteNode(node.Assignment)
                        .GetStoredItem();

        switch (variable)
        {
            case RawValue newVariable:
                var newVal = _runtimeContext.AssignVariable((string)newVariable.GetValue()!, value, node.IsImmutable);
                Result.Set(newVal);
                break;
            case MemoryStorable memoryStorable:
                memoryStorable.SetValue(value);
                Result.Set(memoryStorable);
                break;
        }
    }

    public void Visit(ReferenceNode node)
    {
        var variable = _runtimeContext.GetVariable(node.VariableName);
        Result.Set(new MemoryReference(variable), variable);
    }

    public void Visit(ArrayReferenceNode node)
    {
        var variable = _runtimeContext.GetVariable(node.VariableName);
        Result.Set(new MemoryCollectionReference(variable, NodeToIndexer(node.Index)), variable);
    }

    public void Visit(FieldReferenceNode node)
    {
        var variable = _runtimeContext.GetVariable(node.VariableName);
        Result.Set(new MemoryFieldReference(variable, node.Field), variable);
    }

    private Indexer NodeToIndexer(ArrayIndexingNode node)
    {
        if (node.Sub is not null)
        {
            return new Indexer(Convert.ToInt32(ExecuteNodeAndGetResultValue(node.Index)), NodeToIndexer(node.Sub));
        }

        return new Indexer(Convert.ToInt32(ExecuteNodeAndGetResultValue(node.Index)));
    }

    public void Visit(FunctionDeclarationNode node)
    {
        _runtimeContext.AssignFunction(node.Name, node.Parameters, node.Body);
        Result.ToNull();
    }

    public void Visit(FunctionCallNode node)
    {
        var func = _runtimeContext.GetFunction(node.Name);
        CreateScope(true);

        //Create scoped parameters.
        for (var i = 0; i < func.Parameters.Length; ++i) {
            var param = func.Parameters[i];
            if (node.Parameters.Length - 1 < i)
            {
                throw new MissingParameterException(func.Name, param);
            }

            var value = ExecuteNode(node.Parameters[i])
                            .GetStoredItem();
            if(value is RawValue rawValue)
            {
                value = rawValue.Copy();
            }
            _runtimeContext.AssignVariable(param, value, false);
        }

        //Execute function
        foreach (var bodyNode in func.Body)
        {
            ExecuteNode(bodyNode);
            if (_returnIsCalled)
            {
                break;
            }
        }

        if (_returnIsCalled)
        {
            _returnIsCalled = false;
        }

        PopScope();
    }

    public void Visit(ReturnNode node)
    {
        Result.Set(ExecuteNode(node.ReturnValue).GetStoredItem());
        _returnIsCalled = true;
    }

    public void Visit(NotNode node)
    {
        Result.Set(!(bool)ExecuteNodeAndGetResultValue(node.Condition)!);
    }

    public void Visit(BreakNode _)
    {
        _breakIsCalled = true;
    }

    public void Visit(ConditionalNode node)
    {
        var conditionResult = (bool)ExecuteNodeAndGetResultValue(node.Condition)!;
        if (conditionResult)
        {
            CreateScope();
            foreach (var expr in node.TrueBody) { 
                ExecuteNode(expr);
                if (_returnIsCalled)
                {
                    break;
                }
            }
            PopScope();
        }
        else
        {
            node.Else?.Accept(this);
        }
    }

    public void Visit(GuardNode node)
    {
        var conditionResult = (bool)ExecuteNodeAndGetResultValue(node.Condition)!;
        if (!conditionResult)
        {
            CreateScope();
            foreach (var expr in node.TrueBody)
            {
                ExecuteNode(expr);
                if (_returnIsCalled)
                {
                    break;
                }
            }
            PopScope();
        }
    }

    public void Visit(ForLoopNode node)
    {
        CreateScope();

        var conditionMemoryItem = ExecuteNode(node.Condition).GetStoredItem()!;
        var untilCondition = Convert.ToInt32(ExecuteNodeAndGetResultValue(node.Until));
        var stepSize = Convert.ToInt32(ExecuteNodeAndGetResultValue(node.StepSize));
        while (Convert.ToInt32(conditionMemoryItem.GetValue()) != untilCondition)
        {
            if (_breakIsCalled || _returnIsCalled)
            {
                break;
            }

            foreach(var bodyNode in node.Body)
            {
                if (_breakIsCalled || _returnIsCalled)
                {
                    break;
                }
                ExecuteNode(bodyNode);
            }

            conditionMemoryItem.SetValue(Convert.ToInt32(conditionMemoryItem.GetValue()) + stepSize);
        }

        if (_breakIsCalled)
        {
            _breakIsCalled = false;
        }

        PopScope();
    }

    public void Visit(WhileLoopNode node)
    {
        CreateScope();

        var condition = (bool)ExecuteNodeAndGetResultValue(node.Condition)!;
        while (condition)
        {
            if (_breakIsCalled || _returnIsCalled)
            {
                break;
            }

            foreach (var bodyNode in node.Body)
            {
                if (_breakIsCalled || _returnIsCalled)
                {
                    break;
                }
                ExecuteNode(bodyNode);
            }

            condition = (bool)ExecuteNodeAndGetResultValue(node.Condition)!;
        }

        if (_breakIsCalled)
        {
            _breakIsCalled = false;
        }

        PopScope();
    }

    public void Visit(RepeatLoopNode node)
    {
        CreateScope();

        var condition = (bool)ExecuteNodeAndGetResultValue(node.Condition)!;
        while (!condition)
        {
            if (_breakIsCalled || _returnIsCalled)
            {
                break;
            }

            foreach (var bodyNode in node.Body)
            {
                if (_breakIsCalled || _returnIsCalled)
                {
                    break;
                }
                ExecuteNode(bodyNode);
            }

            condition = (bool)ExecuteNodeAndGetResultValue(node.Condition)!;
        }

        if (_breakIsCalled)
        {
            _breakIsCalled = false;
        }

        PopScope();
    }

    public void Visit(NewInstanceNode node)
    {
        var value = ExecuteNodeAndGetResultValue(node.Operand);
        Result.Set(value);
    }

    public void Visit(ArrayInstanceNode node)
    {
        var dimensions = new List<int>();
        var arrayNode = node;

        do
        {
            dimensions.Add(Convert.ToInt32(ExecuteNodeAndGetResultValue(arrayNode.Size)));
            arrayNode = arrayNode.Sub;
        }
        while (arrayNode is not null);

        Result.Set(ArrayHelper.CreateJaggedArray([.. dimensions], 0));
    }

    public void Visit(StructDeclarationNode node)
    {
        _runtimeContext.CreateStruct(node.Name, node.Fields);
        Result.ToNull();
    }

    public void Visit(StructInstanceNode node)
    {
        var newStructInstance = _runtimeContext.CreateNewStructInstance(node.Name);
        var fields = newStructInstance.GetFields();
        for (var i = 0; i < fields.Count; ++i)
        {
            if(i >= node.Parameters.Length)
            {
                break;
            }

            newStructInstance.SetFieldValue(
                fields.ElementAt(i), 
                ExecuteNodeAndGetResultValue(node.Parameters[i])
            );
        }

        Result.Set(newStructInstance);
    }

    public void Visit(FieldAccessorNode node)
    {
        var variable = ExecuteNodeAndGetResultValue(node.Term) as MemoryStruct;
        var field = variable?.GetFieldValue(node.Name);
        Result.Set(field);
    }

    public void Visit(FieldAssignNode node)
    {
        var variable = ExecuteNodeAndGetResultValue(node.Term) as MemoryStruct;
        var value = ExecuteNodeAndGetResultValue(node.Value)!;
        variable?.SetFieldValue(node.Field, value);
        Result.ToNull();
    }

    public void Visit(ArrayIndexingNode node)
    {
        var array = node;
        var variable = ExecuteNodeAndGetResultValue(node.Operand)! as dynamic;

        var result = variable;
        do
        {
            var indexingResult = ExecuteNodeAndGetResultValue(array.Index)!;
            var index = Convert.ToInt32(indexingResult);
            result = result![index!];
            array = array.Sub;
        }
        while (array is not null);

        Result.Set(result);
    }

    public void Visit(ArrayAssignNode node)
    {
        var currentIndex = node.Index;
        var variable = ExecuteNodeAndGetResultValue(currentIndex.Operand)! as dynamic; //Expected operand is of array type.
        var value = ExecuteNodeAndGetResultValue(node.Value);

        do
        {
            var index = Convert.ToInt32(ExecuteNodeAndGetResultValue(currentIndex.Index));
            if (currentIndex.Sub is null)
            {
                variable![index!] = value;
            }
            else
            {
                variable = variable![index!];
            }
            currentIndex = currentIndex.Sub;
        }
        while (currentIndex is not null);

        Result.ToNull();
    }

    public InterpretResult ExecuteNode(AstNode node)
    {
        node.Accept(this);
        return Result.Copy();
    }

    public object? ExecuteNodeAndGetResultValue(AstNode node)
    {
        var result = ExecuteNode(node)!; 
        return result.GetStoredItem()!.GetValue();
    }

    public RuntimeContext GetRuntimeContext()
    {
        return _runtimeContext;
    }

    private void CreateScope(bool isIsolated = false)
    {
        _runtimeContext = _runtimeContext.CreateChildRuntimeContext(isIsolated);
    }

    private void PopScope()
    {
        _runtimeContext = _runtimeContext.PopRuntimeContext() ?? _runtimeContext;
    }
}
