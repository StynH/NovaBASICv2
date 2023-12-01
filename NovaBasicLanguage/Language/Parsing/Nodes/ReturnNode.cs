using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class ReturnNode(AstNode returnValue) : AstNode
{
    public AstNode ReturnValue { get; set; } = returnValue;
}

