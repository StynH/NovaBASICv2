using NovaBasic.Language.Exceptions;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Exceptions;
using NovaBasicLanguage.Language.Runtime;

namespace NovaBASIC.Language.Runtime;

public class RuntimeContext(RuntimeContext? parent = null)
{
    private Dictionary<string, MemoryItem> _variables = [];
    private Dictionary<string, MemoryReference> _references = [];
    private Dictionary<string, MemoryFunction> _functions = [];

    private readonly RuntimeContext? _parentRuntimeContext = parent; // Parent RuntimeContext for nested scopes

    public void AssignVariable(string variableName, object? value, bool isImmutable)
    {
        if (_references.TryGetValue(variableName, out var reference))
        {
            var memoryItem = GetVariable(reference.VariableName);
            AssignVariable(memoryItem.Name, value, memoryItem.IsImmutable);
            return;
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
            return;
        }

        //Check upper scopes.
        var currentRuntimeContext = _parentRuntimeContext;
        while (currentRuntimeContext != null)
        {
            if (currentRuntimeContext._variables.TryGetValue(variableName, out var parentScopeItem))
            {
                if (parentScopeItem.IsImmutable)
                {
                    throw new MutabilityViolationException(variableName);
                }

                parentScopeItem.Value = value;
                currentRuntimeContext._variables[variableName] = parentScopeItem;
                return;
            }
            currentRuntimeContext = currentRuntimeContext._parentRuntimeContext;
        }

        _variables[variableName] = new MemoryItem(variableName, value, isImmutable);
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
            variableName = reference.VariableName;
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

    public RuntimeContext CreateChildRuntimeContext()
    {
        return new RuntimeContext(this);
    }

    public RuntimeContext? PopRuntimeContext()
    {
        return _parentRuntimeContext;
    }
}