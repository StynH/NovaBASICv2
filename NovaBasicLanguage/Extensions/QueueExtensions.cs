namespace NovaBasicLanguage.Extensions;

public static class QueueExtensions
{
    public static bool NextTokenIs(this Queue<string> tokens, string token)
    {
        return tokens.TryPeek(out var next) && next.Equals(token);
    }
}
