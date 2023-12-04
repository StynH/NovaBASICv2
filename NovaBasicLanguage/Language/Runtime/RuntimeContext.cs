using NovaBasic.Language.Exceptions;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.Runtime;

namespace NovaBASIC.Language.Runtime;

public class RuntimeContext(bool isGlobal = false, bool isIsolated = false, RuntimeContext? parent = null)
{
    private Dictionary<string, MemoryItem> _variables = [];
    private Dictionary<string, MemoryReference> _references = [];
    private Dictionary<string, MemoryFunction> _functions = [];

    private readonly RuntimeContext? _parentRuntimeContext = parent; // Parent RuntimeContext for nested scopes

    public bool IsGlobal { get; } = isGlobal;
    public bool IsIsolated { get; } = isIsolated;

    public MemoryItem AssignVariable(string variableName, object? value, bool isImmutable)
    {
        if (_references.TryGetValue(variableName, out var reference))
        {
            var memoryItem = reference.GetReferencedItem();
            if (reference is MemoryCollectionReference memoryCollectionReference)
            {
                return AssignCollectionIndexedValue(memoryItem, memoryCollectionReference, value);
            }

            return AssignVariable(memoryItem.Name, value, memoryItem.IsImmutable);
        }

        //Variable is in current scope.
        if (_variables.TryGetValue(variableName, out var currentScopeItem))
        {
            if (currentScopeItem.IsImmutable)
            {
                throw new MutabilityViolationException(variableName);
            }

            currentScopeItem.Value = value;
            _variables[variableName] = currentScopeItem;
            return _variables[variableName];
        }

        //Check upper scopes.
        var currentRuntimeContext = _parentRuntimeContext;
        while (currentRuntimeContext != null)
        {
            //Isolated scopes happen during recursion.
            //Isolated scopes can only access global variables.
            if (IsIsolated && !currentRuntimeContext.IsGlobal)
            {
                currentRuntimeContext = currentRuntimeContext._parentRuntimeContext;
                continue;
            }

            if (currentRuntimeContext._variables.TryGetValue(variableName, out var parentScopeItem))
            {
                if (parentScopeItem.IsImmutable)
                {
                    throw new MutabilityViolationException(variableName);
                }

                parentScopeItem.Value = value;
                currentRuntimeContext._variables[variableName] = parentScopeItem;
                return currentRuntimeContext._variables[variableName];
            }
            currentRuntimeContext = currentRuntimeContext._parentRuntimeContext;
        }

        _variables[variableName] = new MemoryItem(variableName, value, isImmutable);
        return _variables[variableName];
    }

    private static MemoryItem AssignCollectionIndexedValue(MemoryItem memoryItem, MemoryCollectionReference memoryCollectionReference, object? value)
    {
        var variable = memoryItem.Value as dynamic;
        var indexer = memoryCollectionReference.Index;

        do
        {
            if (indexer.Sub is null)
            {
                variable![indexer.Index] = value;
                break;
            }

            variable = variable![indexer.Index];
            indexer = indexer.Sub;
        } 
        while (indexer is not null);

        return memoryItem;
    }

    public void AssignVariable(MemoryItem memoryItem)
    {
        AssignVariable(memoryItem.Name, memoryItem.Value, memoryItem.IsImmutable);
    }

    public void AssignReference(string variableName, MemoryReference reference)
    {
        _references[variableName] = reference;
    }

    public MemoryItem GetVariable(string variableName)
    {
        if (_references.TryGetValue(variableName, out var reference))
        {
            return reference.GetReferencedItem();
        }
        if (_variables.TryGetValue(variableName, out var item))
        {
            return item;
        }

        return _parentRuntimeContext?.GetVariable(variableName)
               ?? throw new KeyNotFoundException($"Variable '{variableName}' not found.");
    }

    public void AssignFunction(string name, string[] parameters, AstNode[] body)
    {
        if (_functions.ContainsKey(name))
        {
            throw new FunctionAlreadyExistsException(name);
        }

        _functions[name] = new MemoryFunction(name, parameters, body);
    }


    public MemoryFunction GetFunction(string functionName)
    {
        if (_functions.TryGetValue(functionName, out var item))
        {
            return item;
        }

        return _parentRuntimeContext?.GetFunction(functionName)
               ?? throw new KeyNotFoundException($"Function '{functionName}' not found.");
    }

    public RuntimeContext CreateChildRuntimeContext(bool isIsolated)
    {
        return new RuntimeContext(false, isIsolated, this);
    }

    public RuntimeContext? PopRuntimeContext()
    {
        return _parentRuntimeContext;
    }
}