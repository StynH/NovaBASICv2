namespace NovaBasicLanguage.Language.Runtime;

public abstract class MemoryStorable
{
    protected MemoryStorable()
    {
    }

    public abstract void SetValue(object? value);

    public abstract object? GetValue();
   
}
