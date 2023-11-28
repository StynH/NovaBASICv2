using NovaBASIC.Language.Interpreting;
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

            if(lhs is null)
            {
                throw new ArgumentNullException(nameof(lhs));
            }

            if (rhs is null)
            {
                throw new ArgumentNullException(nameof(lhs));
            }

            switch (binaryNode.Op)
            {
                case Tokens.PLUS:
                    interpreter.Result = lhs + rhs;
                    break;
                case Tokens.MINUS:
                    interpreter.Result = lhs - rhs;
                    break;
                case Tokens.DIVIDE:
                    interpreter.Result = lhs / rhs;
                    break;
                case Tokens.MULTIPLY:
                    interpreter.Result = lhs * rhs;
                    break;
                case Tokens.MODULO:
                    interpreter.Result = lhs % rhs;
                    break;
                case Tokens.EQUALS:
                    interpreter.Result = lhs.Equals(rhs);
                    break;
                case Tokens.NOT_EQUALS:
                    interpreter.Result = !lhs.Equals(rhs);
                    break;
                case Tokens.GTE:
                    interpreter.Result = lhs >= rhs;
                    break;
                case Tokens.LTE:
                    interpreter.Result = lhs <= rhs;
                    break;
                case Tokens.LT:
                    interpreter.Result = lhs < rhs;
                    break;
                case Tokens.GT:
                    interpreter.Result = lhs > rhs;
                    break;
                default:
                    throw new ArithmeticException($"Unknown arithmetic operator '{binaryNode.Op}'.");
            }
        }

        return null;
    }
}
