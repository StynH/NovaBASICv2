using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Interpreting.Safe;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;
using System.Text.RegularExpressions;

namespace NovaBASIC.Language.STL.Functions;

[StlFunction(typeof(BinaryNode))]
public class ComparisonFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if(node is BinaryNode binaryNode)
        {
            if (binaryNode.Op.Equals(Tokens.OR))
            {
                return ExecuteOr(interpreter, binaryNode);
            }

            var lhs = interpreter.ExecuteNodeAndGetResultValue(binaryNode.Left) as dynamic;
            var rhs = interpreter.ExecuteNodeAndGetResultValue(binaryNode.Right) as dynamic;

            if(lhs is object[] lhsArray && rhs is object[] rhsArray)
            {
                return ExecuteArrayOperation(binaryNode.Op, lhsArray, rhsArray);
            }

            return ExecuteOperand(binaryNode.Op, lhs, rhs);
        }

        return null;
    }

    private static object? ExecuteOperand(string op, dynamic lhs, dynamic rhs)
    {
        return op switch
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
            Tokens.AND => lhs && rhs,
            Tokens.BITWISE_AND => lhs & rhs,
            Tokens.BITWISE_OR => lhs | rhs,
            Tokens.BITWISE_XOR => lhs ^ rhs,
            Tokens.BITWISE_LEFT_SHIFT => lhs << rhs,
            Tokens.BITWISE_RIGHT_SHIFT => lhs >> rhs,
            Tokens.MATCHES_STL => Regex.IsMatch(lhs, rhs),
            _ => throw new ArithmeticException($"Unknown arithmetic operator '{op}'."),
        };
    }

    private static object? ExecuteOr(Interpreter interpreter, BinaryNode binaryNode)
    {
        var lhs = interpreter.ExecuteNode(binaryNode.Left) as dynamic;
        if (lhs)
        {
            return true;
        }
        return interpreter.ExecuteNode(binaryNode.Right);
    }

    private object[] ExecuteArrayOperation(string op, object[] lhsArray, object[] rhsArray)
    {
        for (var i = 0; i < lhsArray.Length; i++)
        {
            if (i >= rhsArray.Length)
            {
                break;
            }
            lhsArray[i] = ExecuteOperand(op, lhsArray[i] as dynamic, rhsArray[i] as dynamic);
        }
        return lhsArray;
    }
}
