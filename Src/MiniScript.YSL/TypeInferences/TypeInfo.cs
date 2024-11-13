using TunnelSoft.MiniScript.YSL.Symbols.Base;

namespace TunnelSoft.MiniScript.YSL.TypeInferences;

public class TypeInfo {
    public SymbolType BaseType { get; set; }
    public List<TypeInfo> GenericParameters { get; set; }
    public bool IsNullable { get; set; }
    public HashSet<string> PossibleTypes { get; set; }

    public TypeInfo() {
        GenericParameters = new List<TypeInfo>();
        PossibleTypes = new HashSet<string>();
    }
}