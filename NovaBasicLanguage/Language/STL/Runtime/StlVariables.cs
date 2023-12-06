using NovaBASIC.Language.Runtime;
using NovaBasicLanguage.Language.Runtime;

namespace NovaBasic.Language.STL.Runtime;

public static class StlVariables
{
    public static readonly IList<MemoryItem> STANDARD_VARIABLES = [
        new MemoryItem("M_PI", new RawValue(Math.PI), true),
        new MemoryItem("M_E", new RawValue(Math.E), true),
        new MemoryItem("M_TAU", new RawValue(Math.Tau), true),

        new MemoryItem("NUM_GOLDEN_RATIO", new RawValue((1 + Math.Sqrt(5)) / 2), true),

        new MemoryItem("CS_MAX_INT", new RawValue(int.MaxValue), true),
        new MemoryItem("CS_MIN_INT", new RawValue(int.MinValue), true),
        new MemoryItem("CS_EPSILON", new RawValue(double.Epsilon), true)
    ];
}
