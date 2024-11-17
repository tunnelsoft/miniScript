namespace TunnelSoft.MiniScript.YSL.Test;


using Antlr4.Runtime;

public class ParserTests {
    private MiniScriptParser SetupParser(string input) {
        var lexer = new MiniScriptLexer(new AntlrInputStream(input));
        var tokens = new CommonTokenStream(lexer);
        return new MiniScriptParser(tokens);
    }

    [TestMethod]
    public void TestParser_VariableDeclaration() {
        var parser = SetupParser("var x = 5;");
        var tree = parser.variableDeclaration();
        Assert.NotNull(tree);
        Assert.Equal("var", tree.VAR().GetText());
        Assert.Equal("x", tree.IDENTIFIER().GetText());
        Assert.Equal("5", tree.expression().GetText());
    }

    [TestMethod]
    public void TestParser_IfStatement() {
        var parser = SetupParser("if (x > 3) { print(x); }");
        var tree = parser.ifStatement();
        Assert.NotNull(tree);
        Assert.NotNull(tree.expression());
        Assert.NotNull(tree.block());
    }

    [TestMethod]
    public void TestParser_LoopStatement() {
        var parser = SetupParser("loop (i < 10) { i = i + 1; }");
        var tree = parser.loopStatement();
        Assert.NotNull(tree);
        Assert.NotNull(tree.expression());
        Assert.NotNull(tree.block());
    }

    [TestMethod]
    public void TestParser_ForStatement() {
        var parser = SetupParser("for (var i = 0; i < 10; i = i + 1) { print(i); }");
        var tree = parser.forStatement();
        Assert.NotNull(tree);
        Assert.NotNull(tree.variableDeclaration());
        Assert.NotNull(tree.expression());
        Assert.NotNull(tree.assignment());
        Assert.NotNull(tree.block());
    }
}
