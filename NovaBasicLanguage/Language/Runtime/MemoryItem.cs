using NovaBasicLanguage.Language.Runtime;

namespace NovaBASIC.Language.Runtime;

public class MemoryItem(string name, object? value, bool isImmutable) : IReferencable
{
    public string Name { get; set; } = name;
    public object? Value { get; set; } = value;
    public bool IsImmutable { get; set; } = isImmutable;
}
