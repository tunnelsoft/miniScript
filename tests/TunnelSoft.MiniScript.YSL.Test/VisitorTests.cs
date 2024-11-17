namespace TunnelSoft.MiniScript.YSL.Test;


using Antlr4.Runtime;
using TunnelSoft.YSL.Features.CodeGenerator;

public class VisitorTests {
    private JQueryCodeGeneratorVisitor visitor;

    public VisitorTests() {
        visitor = new JQueryCodeGeneratorVisitor();
    }

    private string GenerateCode(string input) {
        var lexer = new MiniScriptLexer(new AntlrInputStream(input));
        var tokens = new CommonTokenStream(lexer);
        var parser = new MiniScriptParser(tokens);
        var tree = parser.program();
        return visitor.Visit(tree);
    }

    [TestMethod]
    public void TestVisitor_VariableDeclaration() {
        var result = GenerateCode("var x = 5;");
        Assert.Equal("var x = 5;", result.Trim());
    }

    [TestMethod]
    public void TestVisitor_IfStatement() {
        var result = GenerateCode("if (x > 3) { print(x); }");
        Assert.Contains("if (x > 3) {", result);
        Assert.Contains("console.log(x);", result);
        Assert.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_LoopStatement() {
        var result = GenerateCode("loop (i < 10) { i = i + 1; }");
        Assert.Contains("while (i < 10) {", result);
        Assert.Contains("i = i + 1;", result);
        Assert.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_ForStatement() {
        var result = GenerateCode("for (var i = 0; i < 10; i = i + 1) { print(i); }");
        Assert.Contains("for (var i = 0; i < 10; i = i + 1) {", result);
        Assert.Contains("console.log(i);", result);
        Assert.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_FunctionDeclaration() {
        var result = GenerateCode("function add(a, b) { return a + b; }");
        Assert.Contains("function add(a, b) {", result);
        Assert.Contains("return a + b;", result);
        Assert.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_ArrayOperations() {
        var result = GenerateCode("var arr = [1, 2, 3]; arr[1] = 4;");
        Assert.Contains("var arr = [1, 2, 3];", result);
        Assert.Contains("arr[1] = 4;", result);
    }
}
