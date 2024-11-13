using TunnelSoft.MiniScript.YSL.Symbols.Base;

namespace TunnelSoft.MiniScript.YSL.Symbols;

public class EnhancedSymbolTable : SymbolTable {
    private readonly Stack<Dictionary<string, EnhancedSymbol>> enhancedScopes =
        new Stack<Dictionary<string, EnhancedSymbol>>();
    private readonly Dictionary<string, List<string>> dependencies =
        new Dictionary<string, List<string>>();

    public EnhancedSymbolTable() : base() {
        enhancedScopes.Push(new Dictionary<string, EnhancedSymbol>());
    }

    public void DeclareEnhanced(string name, SymbolType type, int declarationLine) {
        var symbol = new EnhancedSymbol(name, type) {
            DeclarationLine = declarationLine
        };
        enhancedScopes.Peek()[name] = symbol;
    }

    public void TrackDependency(string symbol, string dependsOn) {
        if (!dependencies.ContainsKey(symbol)) {
            dependencies[symbol] = new List<string>();
        }
        dependencies[symbol].Add(dependsOn);
    }

    public List<string> GetUnusedSymbols() {
        return enhancedScopes.Peek()
            .Where(s => s.Value.UsageLines.Count == 0)
            .Select(s => s.Key)
            .ToList();
    }

    public Dictionary<string, List<int>> GetSymbolUsageMap() {
        return enhancedScopes.Peek()
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.UsageLines.OrderBy(l => l).ToList()
            );
    }
}
