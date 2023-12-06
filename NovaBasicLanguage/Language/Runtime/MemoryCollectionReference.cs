namespace NovaBasicLanguage.Language.Runtime;

public class MemoryCollectionReference(IReferencable referencable, Indexer index) : MemoryReference(referencable)
{
    public Indexer Index { get; } = index;

    public override void SetValue(object? value)
    {
        switch (Referencable)
        {
            case MemoryStorable memoryStorable:
                var array = memoryStorable!.GetValue() as dynamic;
                var indexer = Index;
                do
                {
                    if (indexer.Sub is null)
                    {
                        array![indexer.Index] = value;
                        break;
                    }

                    array = array![indexer.Index];
                    indexer = indexer.Sub;
                }
                while (indexer is not null);
                break;
        }
    }

    public override object? GetValue()
    {
        switch (Referencable)
        {
            case MemoryStorable memoryStorable:
                var array = memoryStorable!.GetValue() as dynamic;
                var indexer = Index;
                do
                {
                    if (indexer.Sub is null)
                    {
                        return array![indexer.Index];
                    }

                    array = array![indexer.Index];
                    indexer = indexer.Sub;
                }
                while (indexer is not null);
                break;
        }

        return null;
    }
}
