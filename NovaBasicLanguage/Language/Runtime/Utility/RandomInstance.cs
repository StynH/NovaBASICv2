namespace NovaBasicLanguage.Language.Runtime.Utility;

public static class RandomInstance
{
    private static readonly Random _random = new();

    public static int GetRandomInt(int min = 0, int max = int.MaxValue)
    {
        return _random.Next(min, max + 1);
    }

    public static float GetRandomFloat(float min = 0.0f, float max = 1.0f)
    {
        return (float)(min + (max - min) * _random.NextDouble());
    }
}

