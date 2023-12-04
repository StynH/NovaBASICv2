namespace NovaBasicLanguage.Language.Runtime;

public class MemoryCollectionReference(IReferencable referencable, Indexer index) : MemoryReference(referencable)
{
    public Indexer Index { get; } = index;
}
