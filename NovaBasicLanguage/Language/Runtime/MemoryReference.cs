using NovaBASIC.Language.Runtime;

namespace NovaBasicLanguage.Language.Runtime;

public class MemoryReference(IReferencable referencable) : MemoryStorable, IReferencable
{
    public IReferencable Referencable { get; set; } = referencable;

    public MemoryItem GetReferencedItem()
    {
        if(Referencable is MemoryItem memoryItem)
        {
            return memoryItem;
        }

        if(Referencable is MemoryReference memoryReference)
        {
            return memoryReference.GetReferencedItem();
        }

        throw new NullReferenceException(nameof(Referencable));
    }

    public override void SetValue(object? value)
    {
        if (Referencable is MemoryStorable memoryStorable)
        {
            memoryStorable.SetValue(value);
        }
    }

    public override object? GetValue()
    {
        if (Referencable is MemoryStorable memoryStorable)
        {
            return memoryStorable.GetValue();
        }

        return null;
    }
}
