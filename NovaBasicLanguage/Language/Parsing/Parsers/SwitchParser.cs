using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.Parsing;
using NovaBasicLanguage.Extensions;
using NovaBASIC.Language.Exceptions;
using NovaBasicLanguage.Language.Exceptions.Assertion;
using NovaBasicLanguage.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_SWITCH)]
internal class SwitchParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var condition = parser.ParseTernary();
        var terminatedCorrectly = false;
        var cases = new List<CaseNode>();

        CaseNode? defaultStatement = null;
        while (tokens.Count > 0)
        {
            if (tokens.NextTokenIs(Tokens.KEYWORD_END_SWITCH))
            {
                tokens.Dequeue();
                terminatedCorrectly = true;
                break;
            }

            if(tokens.NextTokenIs(Tokens.KEYWORD_CASE))
            {
                cases.Add(ParseCaseStatement(tokens, tokens.Dequeue(), parser));
            }

            if (tokens.NextTokenIs(Tokens.KEYWORD_DEFAULT))
            {
                defaultStatement = ParseDefaultStatement(tokens, tokens.Dequeue(), parser);
            }
        }

        if(!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_SWITCH, Tokens.KEYWORD_END_SWITCH);
        }

        return new SwitchNode(condition, [.. cases], defaultStatement);
    }

    private CaseNode ParseCaseStatement(Queue<string> tokens, string currentToken, Parser parser)
    {
        var terminatedCorrectly = false;
        var condition = parser.ParseTernary();
        var body = new List<AstNode>();

        Assert.NextTokenIsCorrectThenDequeue(tokens, Tokens.KEYWORD_CASE, Tokens.SEMICOLON);

        while(tokens.Count > 0)
        {
            if(tokens.NextTokenIs(Tokens.KEYWORD_BREAK) || tokens.NextTokenIs(Tokens.KEYWORD_RETURN))
            {
                body.Add(parser.ParseTernary());
                terminatedCorrectly = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if(!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_CASE, $"{Tokens.KEYWORD_BREAK} or ${Tokens.KEYWORD_RETURN}");
        }

        return new CaseNode(condition, [.. body]);
    }

    private CaseNode ParseDefaultStatement(Queue<string> tokens, string v, Parser parser)
    {
        var terminatedCorrectly = false;
        var body = new List<AstNode>();

        Assert.NextTokenIsCorrectThenDequeue(tokens, Tokens.KEYWORD_DEFAULT, Tokens.SEMICOLON);

        while (tokens.Count > 0)
        {
            if (tokens.NextTokenIs(Tokens.KEYWORD_BREAK) || tokens.NextTokenIs(Tokens.KEYWORD_RETURN))
            {
                body.Add(parser.ParseTernary());
                terminatedCorrectly = true;
                break;
            }

            body.Add(parser.ParseTernary());
        }

        if (!terminatedCorrectly)
        {
            throw new MalformedStatementException(Tokens.KEYWORD_DEFAULT, $"{Tokens.KEYWORD_BREAK} or ${Tokens.KEYWORD_RETURN}");
        }

        return new CaseNode(null, [.. body]);
    }
}