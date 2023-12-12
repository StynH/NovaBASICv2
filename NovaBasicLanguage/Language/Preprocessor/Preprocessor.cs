using System.Text;

namespace NovaBasicLanguage.Language.Preprocessor;

public class Preprocessor
{
    private static readonly string[] SEPERATORS = ["\r\n", "\r", "\n"];
    private static string FROM_TAG = "FROM";
    private static string EXPORT_START_TAG = "EXPORT";
    private static string EXPORT_END_TAG = "ENDEXPORT";

    public static string PreprocessCode(string code)
    {
        var allLines = code.Split(SEPERATORS, StringSplitOptions.None);

        var importLines = allLines.Where(line => line.StartsWith("IMPORT")).ToList();
        var nonImportLines = allLines.Except(importLines).ToList();

        var preprocessed = string.Empty;
        foreach (var importLine in importLines)
        {
            preprocessed += PreprocessImport(importLine);
        }

        return preprocessed + string.Join(Environment.NewLine, nonImportLines);
    }

    private static string PreprocessImport(string importLine)
    {
        var tokens = new Queue<string>(importLine.Split());
        tokens.Dequeue();

        var fileName = string.Empty;
        var importedValues = new List<string>();
        while(tokens.TryDequeue(out var next))
        {
            if (next.Equals(FROM_TAG))
            {
                fileName = $"{tokens.Dequeue().Replace("\"", string.Empty)}.nova";
                break;
            }
            importedValues.Add(next.Replace("\"", string.Empty).Replace(",", string.Empty));
        }

        return ExtractCodeBlocks(fileName, [.. importedValues]);
    }

    private static string ExtractCodeBlocks(string fileName, string[] importedValues)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"The specified file for import '{fileName}' does not exist.");
            return string.Empty;
        }

        var code = File.ReadAllText(fileName);
        var exportedValues = ExtractExportedValues(code);
        if(exportedValues.Length == 0)
        {
            Console.WriteLine($"The specified file for import '{fileName}' does not contain an EXPORT section.");
            return string.Empty;
        }

        var valuesToBeImported = exportedValues;
        if(importedValues.Length > 0)
        {
            var missingImports = importedValues.Except(exportedValues).ToArray();
            foreach(var missingImport in missingImports)
            {
                Console.WriteLine($"Missing imported value '{missingImport}' from '{fileName}'");
            }
            valuesToBeImported = exportedValues.Intersect(importedValues).ToArray();
        }

        var exportedSection = string.Empty;
        foreach(var valueToBeImported in valuesToBeImported)
        {
            var codeBlock = ExtractCodeBlock(code, valueToBeImported);
            if (codeBlock.Equals(string.Empty))
            {
                Console.WriteLine($"Unable to find code definition for '{valueToBeImported}' in {fileName}");
            }
            exportedSection += codeBlock;
        }

        return exportedSection;
    }

    private static string[] ExtractExportedValues(string code)
    {
        var exportBlock = ExtractExportCodeBlock(code);
        if (exportBlock.Equals(string.Empty))
        {
            return [];
        }

        var tokens = new Queue<string>(exportBlock.Split().Where(str => !str.Equals(string.Empty)));
        var exportedValues = new List<string>();

        tokens.Dequeue();
        while(tokens.TryDequeue(out var next))
        {
            if (next.Equals(EXPORT_END_TAG))
            {
                break;
            }
            exportedValues.Add(next.Replace("\"", string.Empty).Replace(",", string.Empty));
        }

        return [.. exportedValues];
    }

    private static string ExtractExportCodeBlock(string code)
    {
        int startIndex = code.IndexOf(EXPORT_START_TAG, StringComparison.OrdinalIgnoreCase);
        if (startIndex == -1)
        {
            return string.Empty;
        }

        int endIndex = code.IndexOf(EXPORT_END_TAG, startIndex, StringComparison.OrdinalIgnoreCase);
        if (endIndex == -1)
        {
            return string.Empty;
        }

        endIndex += EXPORT_END_TAG.Length;
        return code[startIndex..endIndex];
    }

    private static string ExtractCodeBlock(string code, string value)
    {
        var lines = code.Split(SEPERATORS, StringSplitOptions.None);
        var block = new StringBuilder();
        bool isCapturing = false;

        foreach (var line in lines)
        {
            if (line.Contains($"IMMUTABLE {value}"))
            {
                return line;
            }
            else if (line.Contains($"FUNC {value}") || line.Contains($"STRUCT {value}"))
            {
                isCapturing = true;
            }
            else if ((line.Contains("ENDFUNC") || line.Contains("ENDSTRUCT")) && isCapturing)
            {
                block.AppendLine(line);
                break;
            }

            if (isCapturing)
            {
                block.AppendLine(line);
            }
        }

        return block.ToString();
    }
}