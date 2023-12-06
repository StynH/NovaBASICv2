﻿using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.Parsing.Parsers.Attribute;
using NovaBASIC.Language.Parsing.Parsers.Interface;
using NovaBasicLanguage.Language.Parsing.Nodes.Declarations;

namespace NovaBASIC.Language.Parsing.Parsers;

[NodeParser(Tokens.KEYWORD_LET)]
public class VariableDeclarationParser : INodeParser
{
    public AstNode Parse(Queue<string> tokens, string currentToken, Parser parser)
    {
        var variable = tokens.Dequeue();
        tokens.Dequeue(); //Pop the '='.

        return new VariableDeclarationNode(new ConstantNode<string>(variable), parser.ParseTernary());
    }
}
