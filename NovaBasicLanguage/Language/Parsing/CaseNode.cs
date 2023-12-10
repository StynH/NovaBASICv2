﻿using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBasicLanguage.Language.Parsing;

public class CaseNode(AstNode? condition, AstNode[] body) : AstNode
{
    public AstNode? Condition { get; } = condition;
    public AstNode[] Body { get; } = body;
}
