namespace TunnelSoft.MiniScript.YSL.Test;
using TunnelSoft.YSL.Features.CodeGenerator;

public class ErrorHandlingTests {
    private JQueryCodeGenerator generator;

    public ErrorHandlingTests() {
        generator = new JQueryCodeGenerator();
    }

    [TestMethod]
    public async Task TestErrorHandling_UndeclaredVariable() {
        var result = await generator.GenerateCode("print(undeclaredVar);");
        Assert.IsTrue(result.Success);
        Assert.IsFalse(result.Data.IsCompileSuccess);
        //
        //Assert.Contains("Undefined variable 'undeclaredVar'", result.ErrorMessage);
    }

    [TestMethod]
    public async Task TestErrorHandling_SyntaxError() {
        var result = await generator.GenerateCode("if (x > 3) { print(x); ");
        Assert.IsFalse(result.Data.IsCompileSuccess);
        Debug.Contains("Syntax error", result.Data.CompileErrors);
    }
}
