using NovaBASIC.Language.STL.Attribute;
using NovaBASIC.Language.STL.Functions.Interface;

namespace NovaBASIC.Language.STL;

public class StandardLibrary
{
    private readonly Dictionary<string, IStlFunction> functions = [];

    public StandardLibrary()
    {
        RegisterStandardFunctions();
    }

    private void RegisterStandardFunctions()
    {
        var functionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(StlFunctionAttribute), false).Length > 0);

        foreach (var type in functionTypes)
        {
            var attribute = (StlFunctionAttribute)type.GetCustomAttributes(typeof(StlFunctionAttribute), false).First();
            var functionInstance = Activator.CreateInstance(type) as IStlFunction;
            functions.Add(attribute.FunctionName, functionInstance!);
        }
    }

    public IStlFunction GetFunction(string name)
    {
        if (functions.TryGetValue(name, out var function))
        {
            return function;
        }

        throw new Exception($"Function '{name}' not found in the standard library.");
    }
}
