using NovaBasic.Language.Exceptions;
using NovaBasicLanguage.Language.Runtime;

namespace NovaBASIC.Language.Runtime;

public class MemoryItem(string name, object? value, bool isImmutable) : MemoryStorable, IReferencable
{
    public string Name { get; set; } = name;
    public object? Value { get; set; } = value;
    public bool IsImmutable { get; set; } = isImmutable;

    public override object? GetValue()
    {
        if(Value is MemoryStorable memoryStorable)
        {
            return memoryStorable.GetValue();
        }

        return Value;
    }

    public override void SetValue(object? value)
    {
        if(IsImmutable)
        {
            throw new MutabilityViolationException(Name);
        }

        if (Value is MemoryStorable memoryStorable)
        {
            memoryStorable.SetValue(value);
            return;
        }

        Value = value;
    }
}
