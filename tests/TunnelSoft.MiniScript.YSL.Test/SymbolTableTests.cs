using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelSoft.MiniScript.YSL.Symbols.Base;

namespace TunnelSoft.MiniScript.YSL.Test;



[TestClass]
public class SymbolTableTests {
    private SymbolTable symbolTable;

    public SymbolTableTests() {
        symbolTable = new SymbolTable();
    }


    [TestMethod]
    public void TestSymbolTable_DeclareAndRetrieve() {
        symbolTable.Declare("x", SymbolType.Variable);
        Assert.IsTrue(symbolTable.IsDeclared("x"));
        var symbol = symbolTable.GetSymbol("x");
        Assert.IsNotNull(symbol);
        Assert.AreEqual("x", symbol.Name);
        Assert.AreEqual(SymbolType.Variable, symbol.Type);
    }

    [TestMethod]
    public void TestSymbolTable_Scopes() {
        symbolTable.Declare("x", SymbolType.Variable);
        symbolTable.EnterScope();
        symbolTable.Declare("y", SymbolType.Variable);
        Assert.IsTrue(symbolTable.IsDeclared("x"));
        Assert.IsTrue(symbolTable.IsDeclared("y"));
        symbolTable.ExitScope();
        Assert.IsTrue(symbolTable.IsDeclared("x"));
        Assert.IsFalse(symbolTable.IsDeclared("y"));
    }
}
