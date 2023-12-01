﻿using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Runtime;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.Helpers;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;
using NovaBasicLanguage.Language.Runtime;
using System.Reflection;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    private RuntimeContext _runtimeContext = new();

    private Dictionary<Type, MethodInfo> _methodsCache = new Dictionary<Type, MethodInfo>();

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
                throw new MissingMethodException("No Visit method found for type " + nodeType);
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
        Result = node.Value;
    }

    public void Visit(NullNode node)
    {
        Result = null;
    }

    public void Visit(VariableNode node)
    {
        //TODO: The '.Value' thing should be different.
        Result = _runtimeContext.GetVariable(node.Name).Value;
    }

    public void Visit(VariableDeclarationNode node)
    {
        var name = node.Name;
        var value = ExecuteNode(node.Assignment);

        switch (value)
        {
            case MemoryReference memoryReference:
                _runtimeContext.AssignReference(name, memoryReference);
                Result = null;
                return;
        }

        _runtimeContext.AssignVariable(name, value, node.IsImmutable);
        Result = null;
    }

    public void Visit(ReferenceNode node)
    {
        Result = new MemoryReference(node.VariableName);
    }

    public void Visit(ArrayReferenceNode node)
    {
        Result = new MemoryCollectionReference(node.VariableName, NodeToIndexer(node.Index));
    }

    private Indexer NodeToIndexer(ArrayIndexingNode node)
    {
        if (node.Sub is not null)
        {
            return new Indexer((int)ExecuteNode(node.Index)!, NodeToIndexer(node.Sub));
        }

        return new Indexer((int)ExecuteNode(node.Index)!);
    }

    public void Visit(FunctionDeclarationNode node)
    {
        _runtimeContext.AssignFunction(node.Name, node.Parameters, node.Body);
        Result = null;
    }

    public void Visit(FunctionCallNode node)
    {
        var func = _runtimeContext.GetFunction(node.Name);
        CreateScope();

        //Create scoped parameters.
        for(var i = 0; i < func.Parameters.Length; ++i) {
            var param = func.Parameters[i];
            if(node.Parameters.Length - 1 < i)
            {
                throw new MissingParameterException(func.Name, param);
            }

            var value = ExecuteNode(node.Parameters[i]);
            if(value is MemoryReference memoryReference)
            {
                _runtimeContext.AssignReference(param, memoryReference);
                continue;
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
        else
        {
            Result = null;
        }

        PopScope();
    }

    public void Visit(ReturnNode node)
    {
        _returnIsCalled = true;
        Result = ExecuteNode(node.ReturnValue);
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

    public void Visit(NewInstanceNode node)
    {
        var value = ExecuteNode(node.Operand);
        Result = value;
    }

    public void Visit(ArrayDeclarationNode node)
    {
        var dimensions = new List<int>();
        var arrayNode = node;

        do
        {
            dimensions.Add((int)ExecuteNode(arrayNode.Size)!);
            arrayNode = arrayNode.Sub;
        }
        while (arrayNode is not null);

        Result = ArrayHelper.CreateJaggedArray([.. dimensions], 0);
    }

    public void Visit(ArrayIndexingNode node)
    {
        var array = node;
        var variable = ExecuteNode(node.Operand) as dynamic; //Expected operand is of array type.

        var result = variable;
        do
        {
            var index = (int)ExecuteNode(array.Index)!;
            result = result![index!];
            array = array.Sub;
        }
        while (array is not null);

        Result = result;
    }

    public void Visit(ArrayAssignNode node)
    {
        var currentIndex = node.Index;
        var variable = ExecuteNode(currentIndex.Operand) as dynamic; //Expected operand is of array type.
        var value = ExecuteNode(node.Value);

        do
        {
            var index = (int)ExecuteNode(currentIndex.Index)!;
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
