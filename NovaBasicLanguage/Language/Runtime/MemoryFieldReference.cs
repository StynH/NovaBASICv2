namespace NovaBasicLanguage.Language.Runtime;

public class MemoryFieldReference(IReferencable referencable, string field) : MemoryReference(referencable)
{
    public string Field { get; set; } = field;
}