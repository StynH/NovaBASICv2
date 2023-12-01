namespace NovaBasicLanguage.Language.Exceptions;

public class FunctionAlreadyExistsException(string functionName) : Exception($"Function with name '{functionName}' is already defined.")
{
}
