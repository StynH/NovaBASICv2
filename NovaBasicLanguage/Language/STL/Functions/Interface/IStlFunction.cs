using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Interpreting;

namespace NovaBASIC.Language.STL.Functions.Interface;

public interface IStlFunction
{
    object? Execute(Interpreter interpreter, AstNode node);
}
