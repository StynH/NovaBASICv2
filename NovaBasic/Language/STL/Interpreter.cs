using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Lexicon;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    public void Visit(PrintNode node)
    {
        _stl
            .GetFunction(Tokens.PRINT_STL)
            .Execute(this, node);
    }
}
