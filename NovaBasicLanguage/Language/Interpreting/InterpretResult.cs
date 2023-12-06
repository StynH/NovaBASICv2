
using NovaBASIC.Language.Runtime;
using NovaBasicLanguage.Language.Runtime;

namespace NovaBasicLanguage.Language.Interpreting;

public class InterpretResult
{
    private object? Value { get; set; }
    public object? Operand { get; private set; }

    public void Set(object? value, object? operand = null)
    {
        Value = value;
        Operand = operand;
    }

    public void ToNull()
    {
        Set(null, null);
    }

    public MemoryStorable? GetStoredItem()
    {
        return Value switch
        {
            MemoryStorable memoryStorable => memoryStorable,
            _ => new RawValue(Value),
        };
    }

    public InterpretResult Copy()
    {
        return new InterpretResult { Operand = Operand, Value = Value };
    }
}
