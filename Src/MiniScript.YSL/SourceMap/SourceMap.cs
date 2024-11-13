namespace TunnelSoft.MiniScript.YSL.SourceMap;

public class SourceMap {
    private readonly List<SourceMapEntry> entries = new List<SourceMapEntry>();
    private readonly Dictionary<string, List<int>> symbolUsage = new Dictionary<string, List<int>>();
    private readonly Dictionary<string, List<string>> fileDependencies = new Dictionary<string, List<string>>();

    public void AddMapping(int origLine, int origColumn, int genLine, int genColumn, string sourceFile, string name = null) {
        entries.Add(new SourceMapEntry {
            OriginalLine = origLine,
            OriginalColumn = origColumn,
            GeneratedLine = genLine,
            GeneratedColumn = genColumn,
            SourceFile = sourceFile,
            Name = name
        });
    }

    public void TrackSymbol(string symbol, int line) {
        if (!symbolUsage.ContainsKey(symbol)) {
            symbolUsage[symbol] = new List<int>();
        }
        symbolUsage[symbol].Add(line);
    }

    public void AddFileDependency(string file, string dependency) {
        if (!fileDependencies.ContainsKey(file)) {
            fileDependencies[file] = new List<string>();
        }
        fileDependencies[file].Add(dependency);
    }

    public string GenerateSourceMap() {
        var map = new {
            version = 3,
            file = "output.js",
            sources = entries.Select(e => e.SourceFile).Distinct().ToList(),
            names = entries.Select(e => e.Name).Where(n => n != null).Distinct().ToList(),
            mappings = GenerateVLQMappings(),
            sourceRoot = ""
        };

        return System.Text.Json.JsonSerializer.Serialize(map);
    }

    private string GenerateVLQMappings() {
        // Implement VLQ encoding for mappings
        // This is a complex process, simplified here for brevity
        return string.Join(";", entries.Select(e => $"{e.GeneratedColumn},{e.OriginalLine},{e.OriginalColumn}"));
    }

    public List<int> GetSymbolUsages(string symbol) {
        return symbolUsage.TryGetValue(symbol, out var usages) ? usages : new List<int>();
    }

    public List<string> GetFileDependencies(string file) {
        return fileDependencies.TryGetValue(file, out var deps) ? deps : new List<string>();
    }

    public SourceMapEntry FindOriginalPosition(int generatedLine, int generatedColumn) {
        return entries.FirstOrDefault(e =>
            e.GeneratedLine == generatedLine && e.GeneratedColumn <= generatedColumn);
    }
}
