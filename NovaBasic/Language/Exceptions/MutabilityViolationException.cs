namespace NovaBasic.Language.Exceptions;

public class MutabilityViolationException(string variableName) : Exception($"'{variableName}' is immutable and it's value cannot be changed.")
{
}
