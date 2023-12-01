namespace NovaBasicLanguage.Language.Helpers;

public static class ArrayHelper
{
    public static object CreateJaggedArray(int[] dimensions, int dimensionIndex)
    {
        int length = dimensions[dimensionIndex];
        Array array;

        if (dimensionIndex == dimensions.Length - 1)
        {
            array = new object[length];
        }
        else
        {
            array = Array.CreateInstance(typeof(object), length);
            for (int i = 0; i < length; i++)
            {
                array.SetValue(CreateJaggedArray(dimensions, dimensionIndex + 1), i);
            }
        }

        return array;
    }
}
