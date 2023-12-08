namespace NovaBasicLanguage.Language.Runtime.Indexing;

public class Indexer(int index, Indexer? sub = null)
{
    public int Index { get; } = index;
    public Indexer? Sub { get; } = sub;
}
