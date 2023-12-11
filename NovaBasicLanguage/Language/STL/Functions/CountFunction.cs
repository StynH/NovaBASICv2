using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;
using NovaBasicLanguage.Language.Runtime;
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
            return result switch
            {
                Array arr => arr.Length,
                MemoryStruct memoryStruct => memoryStruct.GetFields().Count,
                string str => str.Length,
                _ => 0
            };
        }

        return null;
    }
}
