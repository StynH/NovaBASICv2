﻿using NovaBasic.Language.STL.Nodes;
using NovaBASIC.Language.Interpreting.Interface;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL;

namespace NovaBASIC.Language.Interpreting;

public partial class Interpreter : INodeVisitor
{
    private readonly StandardLibrary _stl = new();

    public Interpreter()
    {
        _stl.RegisterStandardVariables(_runtimeContext);
    }

    public void Visit(BinaryNode node)
    {
        Result = _stl
            .GetFunction("COMPARISON")
            .Execute(this, node);
    }

    public void Visit(PrintNode node)
    {
        Result = _stl
            .GetFunction(Tokens.PRINT_STL)
            .Execute(this, node);
    }

    public void Visit(TrigonometricNode node)
    {
        Result = _stl
            .GetFunction("TRIGONOMETRIC")
            .Execute(this, node);
    }
}
