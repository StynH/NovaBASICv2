using NovaBasic.Language.Exceptions;
using NovaBasicLanguage.Language.Runtime;

namespace NovaBASIC.Language.Runtime;

public class RuntimeContext(RuntimeContext? parent = null)
{
    private Dictionary<string, MemoryItem> _variables = [];
    private Dictionary<string, MemoryReference> _references = [];
    private readonly RuntimeContext? _parentRuntimeContext = parent; // Parent RuntimeContext for nested scopes

    public void Assign(string variableName, object? value, bool isImmutable)
    {
        if (_references.TryGetValue(variableName, out var reference))
        {
            variableName = reference.VariableName;
            if (!_variables.ContainsKey(variableName))
            {
                throw new KeyNotFoundException($"Variable '{variableName}' not found.");
            }
        }

        if (_variables.ContainsKey(variableName))
        {
            if (_variables[variableName].IsImmutable)
            {
                throw new MutabilityViolationException(variableName);
            }
            _variables[variableName] = new MemoryItem(variableName, value, isImmutable);
            return;
        }

        var currentRuntimeContext = _parentRuntimeContext;
        while (currentRuntimeContext != null)
        {
            if (currentRuntimeContext._variables.TryGetValue(variableName, out var item))
            {
                if (item.IsImmutable)
                {
                    throw new MutabilityViolationException(variableName);
                }

                item.Value = value;
                return;
            }
            currentRuntimeContext = currentRuntimeContext._parentRuntimeContext;
        }

        _variables[variableName] = new MemoryItem(variableName, value, isImmutable);
    }

    public void Assign(MemoryItem memoryItem)
    {
        Assign(memoryItem.Name, memoryItem.Value, memoryItem.IsImmutable);
    }

    public void AssignReference(string variableName, MemoryReference reference)
    {
        _references[variableName] = reference;
    }

    public MemoryItem Get(string variableName)
    {
        if (_references.TryGetValue(variableName, out var reference))
        {
            variableName = reference.VariableName;
        }
        if (_variables.TryGetValue(variableName, out var item))
        {
            return item;
        }

        return _parentRuntimeContext?.Get(variableName)
               ?? throw new KeyNotFoundException($"Variable '{variableName}' not found.");
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