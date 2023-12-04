using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Functions.Interface;
using NovaBASIC.Language.STL.Attribute;
using NovaBasic.Language.STL.Nodes;

namespace NovaBasic.Language.STL.Functions;

[StlFunction("MATHHELPERS")]
public class MathHelpersFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if (node is TrigonometricNode trigonometricNode)
        {
            var operand = interpreter.ExecuteNode(trigonometricNode.Operand) as dynamic;
            switch(trigonometricNode.FuncName)
            {
                case Tokens.SIN_STL:
                    return Math.Sin(operand);
                case Tokens.COS_STL:
                    return Math.Cos(operand);
                case Tokens.TAN_STL:
                    return Math.Tan(operand);
            }
        }

        return null;
    }
}