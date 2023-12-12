namespace NovaBasicLanguage.Extensions;

public static class CollectionExtensions
{
    public static void MoveToFront<T>(this List<T> list, Dictionary<Type, int> priorityMap)
    {
        var itemsWithPriority = new List<(T item, int index)>();

        for (int i = 0; i < list.Count; i++)
        {
            Type itemType = list[i]!.GetType();
            if (priorityMap.ContainsKey(itemType))
            {
                itemsWithPriority.Add((list[i], i));
            }
        }

        itemsWithPriority.Sort((a, b) =>
        {
            int priorityComparison = priorityMap[a.item!.GetType()].CompareTo(priorityMap[b.item!.GetType()]);
            return priorityComparison != 0 ? priorityComparison : a.index.CompareTo(b.index);
        });

        foreach (var (item, originalIndex) in itemsWithPriority)
        {
            list.Remove(item);
            list.Insert(0, item);
        }
    }
}
