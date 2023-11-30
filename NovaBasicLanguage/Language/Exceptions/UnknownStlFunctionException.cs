namespace NovaBasic.Language.Exceptions;

public class UnknownStlFunctionException(string token) : Exception($"Unknown STL function '{token}.")
{
}
