namespace NovaBasicLanguage.Language.Exceptions;

public class UnknownFieldAccessedException(string objectName, string field) : Exception($"Unknown '{field}' for object '{objectName}'.")
{
}
