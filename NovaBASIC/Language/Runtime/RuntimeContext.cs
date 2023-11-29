namespace NovaBASIC.Language.Runtime;

public class RuntimeContext(RuntimeContext? parent = null)
{
    private Dictionary<string, object?> _variables = [];
    private readonly RuntimeContext? _parentRuntimeContext = parent; // Parent RuntimeContext for nested scopes

    public void Assign(string variableName, object? value)
    {
        if (_variables.ContainsKey(variableName))
        {
            _variables[variableName] = value;
            return;
        }

        var currentRuntimeContext = _parentRuntimeContext;
        while (currentRuntimeContext != null)
        {
            if (currentRuntimeContext._variables.ContainsKey(variableName))
            {
                currentRuntimeContext._variables[variableName] = value;
                return;
            }
            currentRuntimeContext = currentRuntimeContext._parentRuntimeContext;
        }

        _variables[variableName] = value;
    }

    public MemoryItem Get(string variableName)
    {
        if (_variables.TryGetValue(variableName, out var value))
        {
            return new MemoryItem(variableName, value);
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