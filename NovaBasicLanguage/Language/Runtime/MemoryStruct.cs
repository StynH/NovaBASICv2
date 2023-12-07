using NovaBasicLanguage.Language.Exceptions;
using System.Collections.Generic;

namespace NovaBasicLanguage.Language.Runtime;

public class MemoryStruct(string name, string[] fields)
{
    public string Name { get; } = name;
    public Dictionary<string, object?> Fields { get; } = fields.ToDictionary(key => key, value => (object?)null);

    public bool ContainsField(string name)
    {
        return Fields.ContainsKey(name);
    }

    public IReadOnlyCollection<string> GetFields()
    {
        return fields.AsReadOnly();
    }

    public void SetFieldValue(string field, object? value)
    {
        if (!ContainsField(field))
        {
            throw new UnknownFieldAccessedException(field, Name);
        }
        Fields[field] = value;
    }

    public object? GetFieldValue(string field)
    {
        if (!ContainsField(field))
        {
            throw new UnknownFieldAccessedException(field, Name);
        }
        return Fields[field];
    }

    public MemoryStruct NewInstance()
    {
        return new(name, fields);
    }

    public override string ToString()
    {
        return $"[{Name}] {{" + Environment.NewLine +
                    string.Join(Environment.NewLine, Fields) + Environment.NewLine +
               "}";
    }

    public object?[] ToArray()
    {
        return Fields.SelectMany(kvp => new[] { new object?[] { kvp.Key, kvp.Value } }).ToArray();
    }
}
