namespace TunnelSoft.MiniScript.YSL.Test;


using Antlr4.Runtime;
using TunnelSoft.YSL.Features.CodeGenerator.Interfaces;
using TunnelSoft.YSL.Features.CodeGenerator;
[TestClass]
public class ParserTests {



    [TestInitialize]
    public void Initialize() {
        
    }


    private MiniScriptParser SetupParser(string input) {
        var lexer = new MiniScriptLexer(new AntlrInputStream(input));
        var tokens = new CommonTokenStream(lexer);
        return new MiniScriptParser(tokens);
    }





    [TestMethod]
    public void TestParser_VariableDeclaration() {
        var parser = SetupParser("var x = 5;");
        var tree = parser.variableDeclaration();
        Assert.IsNotNull(tree);
        Assert.AreEqual("var", tree.VAR().GetText());
        Assert.AreEqual("x", tree.IDENTIFIER().GetText());
        Assert.AreEqual("5", tree.expression().GetText());
    }

    [TestMethod]
    public void TestParser_IfStatement() {
        var parser = SetupParser("if (x > 3) { print(x); }");
        var tree = parser.ifStatement();
        Assert.IsNotNull(tree);
        Assert.IsNotNull(tree.expression());
        Assert.IsNotNull(tree.block());
    }

    [TestMethod]
    public void TestParser_LoopStatement() {
        var parser = SetupParser("loop (i < 10) { i = i + 1; }");
        var tree = parser.loopStatement();
        Assert.IsNotNull(tree);
        Assert.IsNotNull(tree.expression());
        Assert.IsNotNull(tree.block());
    }

    [TestMethod]
    public void TestParser_ForStatement() {
        var parser = SetupParser("for (var i = 0; i < 10; i = i + 1) { print(i); }");
        var tree = parser.forStatement();
        Assert.IsNotNull(tree);
        Assert.IsNotNull(tree.variableDeclaration());
        Assert.IsNotNull(tree.expression());
        Assert.IsNotNull(tree.assignment());
        Assert.IsNotNull(tree.block());
    }
}
