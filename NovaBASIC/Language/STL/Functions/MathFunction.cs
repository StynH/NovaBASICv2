using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Interpreting.Safe;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;

namespace NovaBASIC.Language.STL.Functions;

[StlFunction("MATH")]
public class MathFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if(node is BinaryNode binaryNode)
        {
            var lhs = interpreter.ExecuteNode(binaryNode.Left) as dynamic;
            var rhs = interpreter.ExecuteNode(binaryNode.Right) as dynamic;

            interpreter.Result = binaryNode.Op switch
            {
                Tokens.PLUS => Operations.Add(lhs, rhs),
                Tokens.MINUS => lhs - rhs,
                Tokens.DIVIDE => lhs / rhs,
                Tokens.MULTIPLY => lhs * rhs,
                Tokens.MODULO => lhs % rhs,
                Tokens.EQUALS => lhs == rhs,
                Tokens.NOT_EQUALS => lhs != rhs,
                Tokens.GTE => lhs >= rhs,
                Tokens.LTE => lhs <= rhs,
                Tokens.LT => lhs < rhs,
                Tokens.GT => lhs > rhs,
                _ => throw new ArithmeticException($"Unknown arithmetic operator '{binaryNode.Op}'."),
            };
        }

        return null;
    }
}
