namespace TunnelSoft.MiniScript.YSL.Test;


using Antlr4.Runtime;

[TestClass]
public class LexerTests {



    [TestMethod]
    public void TestLexer_BasicTokens() {
        var input = "var x = 5; if (x > 3) { print(x); }";
        var lexer = new MiniScriptLexer(new AntlrInputStream(input));
        var tokens = lexer.GetAllTokens();

        Assert.AreEqual(18, tokens.Count);
        Assert.AreEqual("var", tokens[0].Text);
        Assert.AreEqual("x", tokens[1].Text);
        Assert.AreEqual("=", tokens[2].Text);
        Assert.AreEqual("5", tokens[3].Text);
        Assert.AreEqual(";", tokens[4].Text);
        Assert.AreEqual("if", tokens[5].Text);
        Assert.AreEqual("(", tokens[6].Text);
        Assert.AreEqual("x", tokens[7].Text);
        Assert.AreEqual(">", tokens[8].Text);
        Assert.AreEqual("3", tokens[9].Text);
        Assert.AreEqual(")", tokens[10].Text);
        Assert.AreEqual("{", tokens[11].Text);
        Assert.AreEqual("print", tokens[12].Text);
        Assert.AreEqual("(", tokens[13].Text);
        Assert.AreEqual("x", tokens[14].Text);
        Assert.AreEqual(")", tokens[15].Text);
        Assert.AreEqual(";", tokens[16].Text);
        Assert.AreEqual("}", tokens[17].Text);
    }
}
