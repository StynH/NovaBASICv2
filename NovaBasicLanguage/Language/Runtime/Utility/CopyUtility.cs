namespace NovaBasicLanguage.Language.Runtime.Utility;

public static class CopyUtility
{
    public static object? CopyObject(object? value)
    {
        if (value == null)
        {
            return null;
        }
        else if (value.GetType().IsArray)
        {
            return DeepCopyArray((Array)value);
        }
        else if (value is ICloneable cloneable)
        {
            return cloneable.Clone();
        }
        else
        {
            return value;
        }
    }

    private static Array DeepCopyArray(Array originalArray)
    {
        Array copiedArray = (Array)Activator.CreateInstance(originalArray.GetType(), originalArray.Length)!;
        for (int i = 0; i < copiedArray.Length; i++)
        {
            object? originalElement = originalArray.GetValue(i);
            object? copiedElement = CopyObject(originalElement);
            copiedArray.SetValue(copiedElement, i);
        }
        return copiedArray;
    }
}
