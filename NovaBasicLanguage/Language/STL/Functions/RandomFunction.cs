﻿using NovaBASIC.Language.Interpreting;
using NovaBASIC.Language.Lexicon;
using NovaBASIC.Language.Parsing.Nodes;
using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;
using NovaBasicLanguage.Language.Runtime.Utility;
using NovaBasicLanguage.Language.STL.Nodes;

namespace NovaBasicLanguage.Language.STL.Functions;

[StlFunction(Tokens.RAND_STL)]
public class RandomFunction : IStlFunction
{
    public object? Execute(Interpreter interpreter, AstNode node)
    {
        if (node is RandomNode randomNode)
        {
            var min = interpreter.ExecuteNode(randomNode.Min);
            var max = interpreter.ExecuteNode(randomNode.Max);

            switch(min)
            {
                case int imin:
                    var imax = (int)max!;
                    return GetRandomInt(imin, imax);
                case float fmin:
                    var fmax = (float)max!;
                    return GetRandomFloat(fmin, fmax);
            }
        }

        return null;
    }

    private static int GetRandomInt(int min, int max)
    {
        return RandomInstance.GetRandomInt(min, max);
    }

    private static float GetRandomFloat(float min, float max)
    {
        return RandomInstance.GetRandomFloat(min, max);
    }
}