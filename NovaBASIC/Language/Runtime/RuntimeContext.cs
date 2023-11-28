namespace NovaBASIC.Language.Runtime;

public class RuntimeContext(RuntimeContext? parent = null)
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();
    private readonly RuntimeContext? parentRuntimeContext = parent; // Parent RuntimeContext for nested scopes

    public void Assign(string variableName, object value)
    {
        if (variables.ContainsKey(variableName))
        {
            variables[variableName] = value;
            return;
        }

        var currentRuntimeContext = parentRuntimeContext;
        while (currentRuntimeContext != null)
        {
            if (currentRuntimeContext.variables.ContainsKey(variableName))
            {
                currentRuntimeContext.variables[variableName] = value;
                return;
            }
            currentRuntimeContext = currentRuntimeContext.parentRuntimeContext;
        }

        variables[variableName] = value;
    }

    public object Get(string variableName)
    {
        if (variables.TryGetValue(variableName, out var value))
        {
            return value;
        }

        return parentRuntimeContext?.Get(variableName)
               ?? throw new KeyNotFoundException($"Variable '{variableName}' not found.");
    }

    public RuntimeContext CreateChildRuntimeContext()
    {
        return new RuntimeContext(this);
    }
}