using NovaBASIC.Language.Lexicon;
using NovaBasicLanguage.Language.Exceptions;

namespace NovaBasicLanguage.Language.Interpreting.Safe;

public class TypeCaster(Type type)
{
    private readonly Type _type = type ?? throw new ArgumentNullException(nameof(type));

    public static bool operator ==(TypeCaster? caster, object? obj)
    {
        if (caster is null)
        {
            return obj is null;
        }

        return caster.Equals(obj);
    }

    public static bool operator !=(TypeCaster? caster, object? obj)
    {
        return !(caster == obj);
    }

    public override bool Equals(object? obj)
    {
        if (obj is TypeCaster otherCaster)
        {
            return _type.Equals(otherCaster._type);
        }

        if (obj is Type otherType)
        {
            return _type.Equals(otherType);
        }

        return obj is not null && _type.Equals(obj.GetType());
    }

    public override int GetHashCode()
    {
        return _type.GetHashCode();
    }

    public static TypeCaster FromTypeName(string typeName)
    {
        return typeName switch
        {
            Tokens.TYPE_INT => new TypeCaster(typeof(int)),
            Tokens.TYPE_FLOAT => new TypeCaster(typeof(float)),
            Tokens.TYPE_STRING => new TypeCaster(typeof(string)),
            Tokens.TYPE_ARRAY => new TypeCaster(typeof(object[])),
            _ => throw new UnknownTypeException(typeName),
        };
    }

    public static bool TryFromTypeName(string typeName, out TypeCaster? typeCaster)
    {
        typeCaster = null;
        switch(typeName)
        {
            case Tokens.TYPE_INT:
                typeCaster = new TypeCaster(typeof(int));
                return true;
            case Tokens.FLOAT_PATTERN:
                typeCaster = new TypeCaster(typeof(float));
                return true;
            case Tokens.TYPE_STRING:
                typeCaster = new TypeCaster(typeof(string));
                return true;
            case Tokens.TYPE_ARRAY:
                typeCaster = new TypeCaster(typeof(object[]));
                return true;
        }

        return false;
    }
}