namespace NovaBasicLanguage.Language.Exceptions;

public class StructAlreadyExistsException(string structName) : Exception($"Struct with name '{structName}' is already defined.")
{
}
