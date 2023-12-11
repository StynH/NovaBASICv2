namespace NovaBasicLanguage.Language.Exceptions;

public class UnknownTypeException(string type) : Exception($"Unknown NovaBASIC type {type}.")
{
}
