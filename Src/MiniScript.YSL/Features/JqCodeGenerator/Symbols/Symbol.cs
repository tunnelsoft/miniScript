namespace TunnelSoft.YSL.Features.CodeGenerator;
using System.Collections.Generic;

// Symbol class to represent variables, functions, inputs
public class Symbol {
    public string Name { get; }
    public SymbolType Type { get; }

    public Symbol(string name, SymbolType type) {
        Name = name;
        Type = type;
    }
}
