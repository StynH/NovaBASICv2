using NovaBASIC.Language.Exceptions;
using NovaBasicLanguage.Extensions;

namespace NovaBasicLanguage.Language.Exceptions.Assertion;

public static class Assert
{
    public static void NextTokenIsCorrectThenDequeue(Queue<string> tokens, string parsedToken, string expectedToken)
    {
        if (!tokens.NextTokenIs(expectedToken))
        {
            throw new MalformedStatementException(parsedToken, expectedToken);
        }
        tokens.Dequeue();
    }
}
