using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;
using NovaBasicLanguage.Language.Runtime;
using NovaBasicLanguage.Language.STL.Nodes;

namespace NovaBasicLanguage.Language.STL.Functions;

[StlFunction(typeof(ToArrayNode))]
public class ToArrayFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if (node is ToArrayNode toArrayNode)
        {
            var value = interpreter.ExecuteNodeAndGetResultValue(toArrayNode.Operand);
            switch (value)
            {
                case string stringValue:
                    return stringValue.ToCharArray();
                case Array arrayValue:
                    return arrayValue;
                case MemoryStruct structValue:
                    return structValue.ToArray();
                default:
                    return new object[] { value };
            }
        }

        return null;
    }
}