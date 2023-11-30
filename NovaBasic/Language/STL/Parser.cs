using NovaBasic.Language.Exceptions;
using NovaBasic.Language.STL.Parsers;
using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBASIC.Language.Parsing;

public partial class Parser
{
    private AstNode ParseStl(string token)
    {
        if(TrigonometricParser.KNOWN_TOKENS.Contains(token))
        {
            return _tokenParsers["TRIGONOMETRIC"].Parse(_tokens, token, this);
        }

        throw new UnknownStlFunctionException(token);
    }

}
