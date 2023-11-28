using NovaBASIC.Extensions;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;

namespace NovaBASIC.Language.Parsing;

public class Parser
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
            nodes.Add(ParseNode());
        }
        return nodes;
    }

    public AstNode ParseNode()
    {
        var term = ParseTerm();

        if (_tokens.TryPeek(out var next) && (next.IsArithmetic() || next.IsEqualityCheck()))
        {
            var op = _tokens.Dequeue();
            return BalanceNode(new BinaryNode(term, op, ParseNode()));
        }

        return term;
    }

    private AstNode ParseTerm()
    {
        var token = _tokens.Dequeue();
        if (_tokenParsers.TryGetValue(token, out INodeParser? parser))
        {
            return parser.Parse(_tokens, token, this);
        }

        if(!token.StartsWith('\"') 
            && !token.EndsWith('\"') 
            && !token.IsNumeric()
        ) {
            return new VariableNode(token);
        }

        return _tokenParsers["CONSTANTS"].Parse(_tokens, token, this);
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
