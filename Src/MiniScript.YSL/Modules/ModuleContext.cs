using TunnelSoft.MiniScript.YSL.Symbols;

namespace TunnelSoft.MiniScript.YSL.Modules;
public class ModuleContext {
    public string ModuleName { get; set; }
    public string FilePath { get; set; }
    public EnhancedSymbolTable LocalSymbols { get; set; }
    public Dictionary<string, string> Imports { get; set; }
    public HashSet<string> Exports { get; set; }
    public Dictionary<string, ModuleContext> Dependencies { get; set; }
    public Dictionary<string, string> ModuleAliases { get; set; }

    public ModuleContext(string name) {
        ModuleName = name;
        LocalSymbols = new EnhancedSymbolTable();
        Imports = new Dictionary<string, string>();
        Exports = new HashSet<string>();
        Dependencies = new Dictionary<string, ModuleContext>();
        ModuleAliases = new Dictionary<string, string>();
    }

    public void AddImport(string symbol, string from) {
        Imports[symbol] = from;
    }

    public void AddExport(string symbol) {
        Exports.Add(symbol);
    }

    public bool IsSymbolAccessible(string symbol) {
        return LocalSymbols.IsDeclared(symbol) || Imports.ContainsKey(symbol);
    }
}
