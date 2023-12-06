using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL;
using NovaBasicLanguage.Language.STL.Nodes;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    private readonly StandardLibrary _stl = new();

    public Interpreter()
    {
        _stl.RegisterStandardVariables(_runtimeContext);
    }

    public void Visit(BinaryNode node)
    {
        ExecuteStlFunction(node, "COMPARISON");
    }

    public void Visit(PrintNode node)
    {
        ExecuteStlFunction(node, Tokens.PRINT_STL);
    }

    public void Visit(TrigonometricNode node)
    {
        ExecuteStlFunction(node, "MATHHELPERS");
    }

    public void Visit(CountNode node)
    {
        ExecuteStlFunction(node, Tokens.COUNT_STL);
    }

    public void Visit(RandomNode node)
    {
        ExecuteStlFunction(node, Tokens.RAND_STL);
    }

    private void ExecuteStlFunction(AstNode node, string functionName)
    {
        Result.Set(_stl
            .GetFunction(functionName)
            .Execute(this, node));
    }
}
