namespace TunnelSoft.MiniScript.YSL.Test;


using Antlr4.Runtime;
using TunnelSoft.YSL.Features.CodeGenerator;


[TestClass]
public class VisitorTests {
    private JQueryCodeGeneratorVisitor visitor;


    [TestInitialize]
    public void Initialize() {
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
        Assert.AreEqual("var x = 5;", result.Trim());
    }

    [TestMethod]
    public void TestVisitor_IfStatement() {
        var result = GenerateCode("if (x > 3) { print(x); }");
        Debug.Contains("if (x > 3) {", result);
        Debug.Contains("console.log(x);", result);
        Debug.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_LoopStatement() {
        var result = GenerateCode("loop (i < 10) { i = i + 1; }");
        Debug.Contains("while (i < 10) {", result);
        Debug.Contains("i = i + 1;", result);
        Debug.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_ForStatement() {
        var result = GenerateCode("for (var i = 0; i < 10; i = i + 1) { print(i); }");
        Debug.Contains("for (var i = 0; i < 10; i = i + 1) {", result);
        Debug.Contains("console.log(i);", result);
        Debug.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_FunctionDeclaration() {
        var result = GenerateCode("function add(a, b) { return a + b; }");
        Debug.Contains("function add(a, b) {", result);
        Debug.Contains("return a + b;", result);
        Debug.Contains("}", result);
    }

    [TestMethod]
    public void TestVisitor_ArrayOperations() {
        var result = GenerateCode("var arr = [1, 2, 3]; arr[1] = 4;");
        Debug.Contains("var arr = [1, 2, 3];", result);
        Debug.Contains("arr[1] = 4;", result);
    }
}
