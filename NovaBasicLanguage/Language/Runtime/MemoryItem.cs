namespace NovaBASIC.Language.Runtime;

public struct MemoryItem(string name, object? value, bool isImmutable)
{
    public string Name { get; set; } = name;
    public object? Value { get; set; } = value;
    public bool IsImmutable { get; set; } = isImmutable;
}
