namespace NovaBasicLanguage.Language.Runtime;

public class MemoryReference(string variableName)
{
    public string VariableName { get; set; } = variableName;
}
