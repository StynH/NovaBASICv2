using NovaBASIC.Language.Runtime;

namespace NovaBasic.Language.STL.Runtime;

public static class StlVariables
{
    public static readonly IList<MemoryItem> STANDARD_VARIABLES = [
        new MemoryItem("M_PI", Math.PI, true),
        new MemoryItem("M_E", Math.E, true),
        new MemoryItem("M_TAU", Math.Tau, true),

        new MemoryItem("NUM_GOLDEN_RATIO", (1 + Math.Sqrt(5)) / 2, true),

        new MemoryItem("CS_MAX_INT", int.MaxValue, true),
        new MemoryItem("CS_MIN_INT", int.MinValue, true),
        new MemoryItem("CS_EPSILON", double.Epsilon, true)
    ];
}
