namespace NovaBasicLanguage.Language.Runtime;

public class MemoryCollectionReference(string variableName, Indexer index) : MemoryReference(variableName)
{
    public Indexer Index { get; } = index;
}
