using NovaBASIC.Extensions;
using System.Text.RegularExpressions;

namespace NovaBASIC.Language.Lexicon;

public class Lexer
{
    private Queue<string> _tokens = new Queue<string>();

    public void LoadCodeIntoLexer(string code) {
        new Regex(Tokens.BuildRegexPattern())
            .Matches(code.RemoveCommentLines())
            .Select(x => x.Value)
            .Where(x => x != Environment.NewLine)
            .ToList()
            .ForEach(_tokens.Enqueue);
    }

    public Queue<string> GetTokens()
    {
        return _tokens;
    }
}
