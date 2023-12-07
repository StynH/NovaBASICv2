using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    private readonly StandardLibrary _stl = new();

    public Interpreter()
    {
        _stl.RegisterStandardVariables(_runtimeContext);
    }

    public void VisitStl(AstNode node)
    {
        if (_stl.TryGetFunction(node.GetType(), out var stlFunction))
        {
            Result.Set(stlFunction!.Execute(this, node));
        }
        else
        {
            throw new MissingMethodException("No Visit method found for type " + node.GetType());
        }
    }
}
