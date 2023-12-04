using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing;

namespace NovaBasicDesktopRunner;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Please provide a .nova file as an argument.");
            return;
        }

        string filename = args[0];

        if (Path.GetExtension(filename) != ".nova")
        {
            Console.WriteLine("The file must have a .nova extension.");
            return;
        }

        if (!File.Exists(filename))
        {
            Console.WriteLine("The specified file does not exist.");
            return;
        }

        try
        {
            string code = File.ReadAllText(filename);

            var lexer = new Lexer();
            lexer.LoadCodeIntoLexer(code);

            var parser = new Parser(lexer);
            var program = parser.GetProgram();

            var interpreter = new Interpreter();
            interpreter.InitializeMethodCache();
            interpreter.RunProgram(program);

            Console.WriteLine("Program executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
