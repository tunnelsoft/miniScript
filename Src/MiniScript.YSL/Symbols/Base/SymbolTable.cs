namespace TunnelSoft.MiniScript.YSL.Symbols.Base;
using System.Collections.Generic;

public class SymbolTable {


    private int ScopeCounter = 0;

    private readonly Stack<Dictionary<string, Symbol>> scopes = new Stack<Dictionary<string, Symbol>>();

    public SymbolTable() {
        // Initialize with global scope
        scopes.Push(new Dictionary<string, Symbol>());
    }

    // Enter a new scope
    public void EnterScope() {
        ScopeCounter++;
        scopes.Push(new Dictionary<string, Symbol>());
    }

    // Exit the current scope
    public void ExitScope() {
        if (scopes.Count > 1) {
            ScopeCounter--;
            scopes.Pop();
        } else
            throw new InvalidOperationException("Cannot exit the global scope.");
    }

    // Declare a new symbol in the current scope
    public bool Declare(string name, SymbolType type) {
        if (scopes.Peek().ContainsKey(name))
            return false; // Already declared in the current scope

        scopes.Peek()[name] = new Symbol(name, type);
        return true;
    }

    // Check if a symbol is declared in any scope
    public bool IsDeclared(string name) {
        foreach (var scope in scopes) {
            if (scope.ContainsKey(name))
                return true;
        }
        return false;
    }

    // Check if a symbol is declared in the current scope
    public bool IsDeclaredInCurrentScope(string name) {
        return scopes.Peek().ContainsKey(name);
    }

    // Get the symbol from the nearest scope
    public Symbol? GetSymbol(string name) {
        foreach (var scope in scopes) {
            if (scope.ContainsKey(name))
                return scope[name];
        }
        return null;
    }


    public Dictionary<string, Symbol> GetGlobalScope() {
        return scopes.LastOrDefault() ?? new Dictionary<string, Symbol>();
    }

}