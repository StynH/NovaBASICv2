using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Language.Preprocessor;

namespace NovaBasicDesktopRunner;

internal class Program
{
    static void Main(string[] args)
    {
        string? filename = args.FirstOrDefault(
                                arg => Path.GetExtension(arg)
                                            .Equals(".nova", StringComparison.OrdinalIgnoreCase)
                                );

        if (string.IsNullOrEmpty(filename))
        {
#if DEBUG
            filename = "debug.nova";
#else
            Console.WriteLine("Please provide a .nova file as an argument.");
            WaitForInput();
            return;
#endif
        }

        if (!File.Exists(filename))
        {
            Console.WriteLine("The specified file does not exist.");
            WaitForInput();
            return;
        }

        try
        {
            string code = Preprocessor.PreprocessCode(File.ReadAllText(filename));

            var lexer = new Lexer();
            lexer.LoadCodeIntoLexer(code);

            var parser = new Parser(lexer);
            var program = parser.GetProgram();

            var interpreter = new Interpreter();
            interpreter.InitializeMethodCache();
            interpreter.RunProgram(program);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Program executed successfully.");
            WaitForInput();
        }
        catch (Exception ex)
        {
            while(ex!.InnerException is null)
            {
                ex = ex.InnerException!;
            }

            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ReadKey();
        }
    }

    private static void WaitForInput()
    {
        Console.WriteLine("Press any key to close the CLI...");
        Console.ReadKey();
    }
}
