using NovaBasicLanguage.Language.Runtime.Utility;

namespace NovaBasicLanguage.Language.Runtime;

public class RawValue(object? value) : MemoryStorable
{
    public object? Value { get; set; } = value;

    public override object? GetValue()
    {
        return Value;
    }

    public override void SetValue(object? value)
    {
        if(value is RawValue rawValue)
        {
            Value = rawValue.Value;
            return;
        }

        Value = value;
    }

    public RawValue Copy()
    {
        return new RawValue(CopyUtility.CopyObject(Value));
    }

}
