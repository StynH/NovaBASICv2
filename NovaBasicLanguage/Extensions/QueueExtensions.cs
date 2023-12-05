namespace NovaBasicLanguage.Extensions;

public static class QueueExtensions
{
    public static bool NextTokenIs(this Queue<string> tokens, string token)
    {
        return tokens.TryPeek(out var next) && next.Equals(token);
    }

    public static bool NextTokenIs(this Queue<string> tokens,Func<string, bool> filter)
    {
        return tokens.TryPeek(out var next) && filter(next);
    }
}
