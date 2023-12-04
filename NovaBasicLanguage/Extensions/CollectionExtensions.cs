namespace NovaBasicLanguage.Extensions;

public static class CollectionExtensions
{
    public static void MoveToFront<T>(this List<T> list, Func<T, bool> filter)
    {
        int insertIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (filter(list[i]))
            {
                T item = list[i];
                list.RemoveAt(i);
                list.Insert(insertIndex, item);
                insertIndex++;
                if (i > insertIndex) i--;
            }
        }
    }
}
