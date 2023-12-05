namespace NovaBasicLanguage.Language.Exceptions;

public class UnknownStructDeclarationException(string structName) : Exception($"Unknown struct '{structName}'.")
{
}
