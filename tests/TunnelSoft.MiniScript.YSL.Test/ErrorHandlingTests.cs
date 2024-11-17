namespace TunnelSoft.MiniScript.YSL.Test;
using TunnelSoft.YSL.Features.CodeGenerator;

[TestClass]
public class ErrorHandlingTests {
    private JQueryCodeGenerator generator;


    [TestInitialize]
    public void Initialize() {
        generator = new JQueryCodeGenerator();
    }

    [TestMethod]
    public async Task TestErrorHandling_UndeclaredVariable() {
        var result = await generator.GenerateCode("print(undeclaredVar);");
        Assert.IsTrue(result.Success);
        Assert.IsFalse(result.Data.IsCompileSuccess);
        
        Debug.Contains("Undefined variable 'undeclaredVar'", result.Data.CompileErrors);
    }

    [TestMethod]
    public async Task TestErrorHandling_SyntaxError() {
        var result = await generator.GenerateCode("if (x > 3) { print(x); ");
        Assert.IsFalse(result.Data.IsCompileSuccess);
        Debug.Contains("Compilation Errors:", result.Data.CompileErrors);
    }
}
