using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;

namespace NovaBasic.Language.STL.Functions;

[StlFunction(Tokens.PRINT_STL)]
public class PrintFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if(node is PrintNode printNode)
        {
            var result = interpreter.ExecuteNode(printNode.Message)?.ToString();
            Console.WriteLine(result); //TODO: Print to the actual webpage.
        }

        return null;
    }
}
