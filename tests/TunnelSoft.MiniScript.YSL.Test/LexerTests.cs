namespace TunnelSoft.MiniScript.YSL.Test;


using Antlr4.Runtime;

[TestClass]
public class LexerTests {
    [TestMethod]
    public void TestLexer_BasicTokens() {
        var input = "var x = 5; if (x > 3) { print(x); }";
        var lexer = new MiniScriptLexer(new AntlrInputStream(input));
        var tokens = lexer.GetAllTokens();

        Assert.Equal(17, tokens.Count);
        Assert.Equal("var", tokens[0].Text);
        Assert.Equal("x", tokens[1].Text);
        Assert.Equal("=", tokens[2].Text);
        Assert.Equal("5", tokens[3].Text);
        Assert.Equal(";", tokens[4].Text);
        Assert.Equal("if", tokens[5].Text);
        Assert.Equal("(", tokens[6].Text);
        Assert.Equal("x", tokens[7].Text);
        Assert.Equal(">", tokens[8].Text);
        Assert.Equal("3", tokens[9].Text);
        Assert.Equal(")", tokens[10].Text);
        Assert.Equal("{", tokens[11].Text);
        Assert.Equal("print", tokens[12].Text);
        Assert.Equal("(", tokens[13].Text);
        Assert.Equal("x", tokens[14].Text);
        Assert.Equal(")", tokens[15].Text);
        Assert.Equal("}", tokens[16].Text);
    }
}
