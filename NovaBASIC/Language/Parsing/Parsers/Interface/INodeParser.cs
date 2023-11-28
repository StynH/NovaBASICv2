using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBASIC.Language.Parsing.Parsers.Interface;

public interface INodeParser
{
    AstNode Parse(Queue<string> tokens, string currentToken);
}