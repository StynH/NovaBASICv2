using NovaBASIC.Language.Runtime;

namespace NovaBasicLanguage.Language.Runtime;

public class MemoryReference(IReferencable referencable) : IReferencable
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
}
