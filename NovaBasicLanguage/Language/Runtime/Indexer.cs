namespace NovaBasicLanguage.Language.Runtime;

public class Indexer(int index, Indexer? sub = null)
{
    public int Index { get; } = index;
    public Indexer? Sub { get; } = sub;
}
