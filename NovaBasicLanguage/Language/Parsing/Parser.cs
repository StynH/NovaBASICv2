using NovaBASIC.Extensions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBASIC.Language.STL;
using NovaBasicLanguage.Extensions;
using NovaBasicLanguage.Language.Parsing.Nodes;
using NovaBasicLanguage.Language.Parsing.Nodes.Array;
using NovaBasicLanguage.Language.Parsing.Nodes.Declarations;
using NovaBasicLanguage.Language.Parsing.Parsers;

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
        nodes.MoveToFront(new Dictionary<Type, int>
        {
            { typeof(StructDeclarationNode), 1 },
            { typeof(FunctionDeclarationNode), 2 }
        });
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

        while (_tokens.TryPeek(out var next))
        {
            switch (next)
            {
                case Tokens.OPENING_BRACKET:
                    term = ParseArrayIndexing(term);
                    break;

                case var op when next.IsArithmetic() || next.IsEqualityCheck() || next.IsBitwiseOperator() || next.IsSTLOperation():
                    _tokens.Dequeue();
                    term = BalanceNode(new BinaryNode(term, op, ParseTerm()));
                    break;

                case var _ when next.Equals(Tokens.KEYWORD_IS):
                    _tokens.Dequeue();
                    term = new InstanceOfNode(term, _tokens.Dequeue());
                    break;

                default:
                    return term;
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

        if (token.IsSliceIndexer(_tokens))
        {
            return _tokenParsers["SLICING_INDEX"].Parse(_tokens, token, this);
        }

        if (token.IsType())
        {
            return ParseType(token);
        }

        if (token.IsVariable())
        {
            return ProcessVariableToken(token);
        }

        return _tokenParsers["CONSTANTS"].Parse(_tokens, token, this);
    }

    private static TypeNode ParseType(string token)
    {
        return new TypeNode(token);
    }

    private AstNode ProcessVariableToken(string token)
    {
        AstNode term = new VariableNode(token);
        while (_tokens.TryPeek(out var next))
        {
            switch (next)
            {
                case Tokens.OPENING_PARENTHESIS:
                    term = _tokenParsers["FUNC_CALL"].Parse(_tokens, token, this);
                    break;

                case Tokens.OPENING_BRACKET:
                    term = ProcessArrayCallOrAssignment(term);
                    break;

                case Tokens.ACCESSOR:
                    term = ProcessFieldCallOrAssignment(term);
                    break;

                case Tokens.SET:
                    _tokens.Dequeue();
                    term = new VariableDeclarationNode(term, ParseTernary());
                    break;

                default:
                    return term;
            }
        }

        return term;
    }

    private AstNode ProcessArrayCallOrAssignment(AstNode term)
    {
        var arrayIndexing = ParseArrayIndexing(term);

        if (_tokens.NextTokenIs(Tokens.SET))
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

        if (_tokens.NextTokenIs(Tokens.OPENING_BRACKET))
        {
            return new ArrayIndexingNode(term, index, ParseArrayIndexing(term));
        }

        return new ArrayIndexingNode(term, index, null);
    }

    private AstNode ProcessFieldCallOrAssignment(AstNode term)
    {
        _tokens.Dequeue(); //Pop '.' (Accessor).
        var fieldIndexing = _tokens.Dequeue();
        if (_tokens.NextTokenIs(Tokens.SET))
        {
            _tokens.Dequeue(); // Pop '='.
            return new FieldAssignNode(term, fieldIndexing, ParseTernary());
        }

        return new FieldAccessorNode(term, fieldIndexing);
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

            if (binaryNode.Left is BinaryNode leftBinaryNode)
            {
                var rightPrecedence = GetPrecedence(binaryNode.Op);
                var leftPrecedence = GetPrecedence(leftBinaryNode.Op);

                if (rightPrecedence > leftPrecedence)
                {
                    var newLeft = leftBinaryNode;
                    binaryNode.Left = newLeft.Right;
                    newLeft.Right = binaryNode;
                    return newLeft;
                }
            }
        }
        return node;
    }

    private static int GetPrecedence(string op)
    {
        return op switch
        {
            Tokens.AND or Tokens.OR => 0,
            Tokens.EQUALS or Tokens.NOT_EQUALS or Tokens.GT or Tokens.GTE or Tokens.LT or Tokens.LTE => 1,
            Tokens.PLUS or Tokens.MINUS => 2,
            Tokens.MULTIPLY or Tokens.DIVIDE or Tokens.MODULO => 3,
            Tokens.BITWISE_OR or Tokens.BITWISE_AND or Tokens.BITWISE_NOT or Tokens.BITWISE_XOR or Tokens.BITWISE_LEFT_SHIFT or Tokens.BITWISE_RIGHT_SHIFT => 4,
            _ => -1,
        };
    }
}
