using TunnelSoft.MiniScript.YSL.Symbols.Base;

namespace TunnelSoft.MiniScript.YSL.Symbols;

public class EnhancedSymbol : Symbol {
    public int DeclarationLine { get; set; }
    public int LastUsedLine { get; set; }
    public HashSet<int> UsageLines { get; set; }
    public bool IsModified { get; set; }
    public SymbolType InferredType { get; set; }
    public string InitialValue { get; set; }
    public string CurrentValue { get; set; }
    public HashSet<string> Dependencies { get; set; }

    public EnhancedSymbol(string name, SymbolType type) : base(name, type) {
        UsageLines = new HashSet<int>();
        Dependencies = new HashSet<string>();
    }

    public void AddUsage(int line) {
        UsageLines.Add(line);
        LastUsedLine = Math.Max(LastUsedLine, line);
    }
}
