using NovaBASIC.Extensions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.STL;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;

namespace NovaBASIC.Language.Parsing;

public partial class Parser
{
    private readonly Dictionary<string, INodeParser> _tokenParsers = [];
    private readonly Queue<string> _tokens;

    public Parser(Lexer lexer)
    {
        _tokens = lexer.GetTokens();
        RegisterTokenParsers();
    }

    private void RegisterTokenParsers()
    {
        var parserTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(NodeParserAttribute), false).Length > 0);

        foreach (var type in parserTypes)
        {
            var attribute = (NodeParserAttribute)type.GetCustomAttributes(typeof(NodeParserAttribute), false).First();
            if (Activator.CreateInstance(type) is not INodeParser parserInstance)
            {
                continue;
            }

            _tokenParsers.Add(attribute.TokenType, parserInstance);
        }
    }

    public List<AstNode> GetProgram()
    {
        var nodes = new List<AstNode>();
        while (_tokens.Count > 0)
        {
            nodes.Add(ParseTernary());
        }
        return nodes;
    }

    public AstNode ParseTernary()
    {
        var term = ParseBinary();

        // Check for ternary operations
        if (_tokens.TryPeek(out var next) && next.IsOrAnd())
        {
            var op = _tokens.Dequeue();
            term = BalanceNode(new BinaryNode(term, op, ParseTernary()));
        }

        return term;
    }

    public AstNode ParseBinary()
    {
        var term = ParseTerm();

        if (_tokens.TryPeek(out var next))
        {
            switch (next)
            {
                case Tokens.OPENING_BRACKET:
                    term = ParseArrayIndexing(term);
                    break;

                case var op when next.IsArithmetic() || next.IsEqualityCheck() || next.IsSTLOperation():
                    _tokens.Dequeue();
                    term = BalanceNode(new BinaryNode(term, op, ParseTerm()));
                    break;
            }
        }

        return term;
    }

    public AstNode ParseTerm()
    {
        var token = _tokens.Dequeue();

        if (_tokenParsers.TryGetValue(token, out var parser))
        {
            return parser.Parse(_tokens, token, this);
        }

        if (StandardLibrary.IsKnownToken(token))
        {
            return ParseStl(token);
        }

        if (token.IsVariable())
        {
            return ProcessVariableToken(token);
        }

        return _tokenParsers["CONSTANTS"].Parse(_tokens, token, this);
    }

    private AstNode ProcessVariableToken(string token)
    {
        if (_tokens.TryPeek(out var next))
        {
            switch (next)
            {
                case Tokens.OPENING_PARENTHESIS:
                    return _tokenParsers["FUNC_CALL"].Parse(_tokens, token, this);

                case Tokens.OPENING_BRACKET:
                    return ProcessArrayCallOrAssignment(token);

                case Tokens.SET:
                    _tokens.Dequeue();
                    return new VariableDeclarationNode(token, ParseTernary());
            }
        }

        return new VariableNode(token);
    }

    private AstNode ProcessArrayCallOrAssignment(string token)
    {
        var arrayIndexing = ParseArrayIndexing(new VariableNode(token));

        if (_tokens.TryPeek(out var nextToken) && nextToken == Tokens.SET)
        {
            _tokens.Dequeue(); // Pop '='.
            return new ArrayAssignNode(arrayIndexing, ParseTernary());
        }

        return arrayIndexing;
    }

    private ArrayIndexingNode ParseArrayIndexing(AstNode term)
    {
        _tokens.Dequeue(); //Pop '['.
        var index = ParseTernary();
        _tokens.Dequeue(); //Pop ']'.

        if (_tokens.TryPeek(out var next))
        {
            if(next == Tokens.OPENING_BRACKET)
            {
                return new ArrayIndexingNode(term, index, ParseArrayIndexing(term));
            }
        }

        return new ArrayIndexingNode(term, index, null);
    }

    private static AstNode BalanceNode(AstNode node)
    {
        if (node is BinaryNode binaryNode)
        {
            binaryNode.Left = BalanceNode(binaryNode.Left);
            binaryNode.Right = BalanceNode(binaryNode.Right);

            if(binaryNode.Right is BinaryNode rightBinaryNode)
            {
                var rightPrecedence = GetPrecedence(rightBinaryNode.Op);
                var leftPrecedence = GetPrecedence(binaryNode.Op);

                if(rightPrecedence < leftPrecedence)
                {
                    var newRight = rightBinaryNode;
                    binaryNode.Right = newRight.Left;
                    newRight.Left = binaryNode;
                    return newRight;
                }
            }
        }
        return node;
    }

    private static int GetPrecedence(string op)
    {
        return op switch
        {
            Tokens.EQUALS or Tokens.NOT_EQUALS or Tokens.GT or Tokens.GTE or Tokens.LT or Tokens.LTE or Tokens.AND or Tokens.OR => 0,
            Tokens.PLUS or Tokens.MINUS => 1,
            Tokens.MULTIPLY or Tokens.DIVIDE or Tokens.MODULO => 2,
            _ => -1,
        };
    }
}
