namespace NovaBasicLanguage.Language.Runtime.Indexing;

internal class SlicingIndexer(int begin, int end, int step) : IArrayManipulationIndexer
{
    public int Begin { get; } = begin;
    public int End { get; } = end;
    public int Step { get; } = step;

    public object? HandleArray(object[] array)
    {
        var newArray = new List<object>();
        for (var i = Begin; i < End; i += Step)
        {
            if (i >= array.Length)
            {
                return newArray.ToArray();
            }

            newArray.Add(array[i]);
        }

        return newArray.ToArray();
    }
}
