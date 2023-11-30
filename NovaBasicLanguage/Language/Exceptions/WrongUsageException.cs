namespace NovaBasicLanguage.Language.Exceptions;

public class WrongUsageException(string operand, string usage) : Exception($"Wrong usage of '{operand}. '{operand} only works with '{usage}'.")
{
}
