using System.Text.RegularExpressions;

namespace NovaBASIC.Language.Lexicon;

public class Lexer
{
    private Queue<string> _tokens = new Queue<string>();

    public void LoadCodeIntoLexer(string code) {
        new Regex(Tokens.BuildRegexPattern())
            .Matches(code)
            .Select(x => x.Value)
            .Where(x => x != Environment.NewLine)
            .ToList()
            .ForEach(x => _tokens.Enqueue(x));
    }

    public Queue<string> GetTokens()
    {
        return _tokens;
    }
}
