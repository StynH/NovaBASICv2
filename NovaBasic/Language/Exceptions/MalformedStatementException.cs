namespace NovaBASIC.Language.Exceptions;

public class MalformedStatementException(string statement, string missing) : Exception($"Malformed statement for '{statement}, missing '{missing}'.")
{
}
