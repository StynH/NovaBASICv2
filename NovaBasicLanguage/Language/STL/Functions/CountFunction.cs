using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;
using NovaBasicLanguage.Language.STL.Nodes;

namespace NovaBasic.Language.STL.Functions;

[StlFunction(typeof(CountNode))]
public class CountFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if (node is CountNode countNode)
        {
            var result = interpreter.ExecuteNodeAndGetResultValue(countNode.Operand)!;
            var arr = result as Array;
            if (arr != null)
            {
                return arr.Length;
            }

            return 0;
        }

        return null;
    }
}
