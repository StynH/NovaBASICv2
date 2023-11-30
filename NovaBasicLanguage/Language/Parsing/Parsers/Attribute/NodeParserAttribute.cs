namespace NovaBASIC.Language.Parsing.Parsers.Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class NodeParserAttribute(string tokenType) : System.Attribute
{
    public string TokenType { get; private set; } = tokenType;
}