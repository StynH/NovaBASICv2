using NovaBasic.Language.Exceptions;
using NovaBasic.Language.STL.Parsers;
using NovaBASIC.Language.Parsing.Nodes;

namespace NovaBASIC.Language.Parsing;

public partial class Parser
{
    private AstNode ParseStl(string token)
    {
        if(MathHelperFunctionsParser.KNOWN_TOKENS.Contains(token))
        {
            return _tokenParsers["MATHHELPERS"].Parse(_tokens, token, this);
        }

        throw new UnknownStlFunctionException(token);
    }

}
