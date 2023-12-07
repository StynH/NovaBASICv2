using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing.Nodes;

public class GuardNode(AstNode condition, IList<AstNode> trueBody) : AstNode
{
    public AstNode Condition { get; set; } = condition;
    public IList<AstNode> TrueBody { get; set; } = trueBody;
}
