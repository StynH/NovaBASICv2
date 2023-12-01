namespace NovaBasicLanguage.Language.Exceptions;

public class MissingParameterException(string function, string parameter) : Exception($"Missing parameter '{parameter}' for function '{function}'.")
{
}
