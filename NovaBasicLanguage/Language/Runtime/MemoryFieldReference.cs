using NovaBasicLanguage.Language.Exceptions;

namespace NovaBasicLanguage.Language.Runtime;

public class MemoryFieldReference(IReferencable referencable, string field) : MemoryReference(referencable)
{
    public string Field { get; set; } = field;

    public override void SetValue(object? value)
    {
        switch (Referencable)
        {
            case MemoryStorable memoryStorable:
                memoryStorable.SetValue(value);
                break;
            case MemoryStruct memoryStruct:
                memoryStruct.SetFieldValue(field, value);
                break;
        }
    }

    public override object? GetValue()
    {
        switch (Referencable)
        {
            case MemoryStorable memoryStorable:
                return memoryStorable.GetValue();
            case MemoryStruct memoryStruct:
                return memoryStruct.GetFieldValue(field);
        }

        return null;
    }
}