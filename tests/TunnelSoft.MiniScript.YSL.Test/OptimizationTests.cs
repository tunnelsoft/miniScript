namespace TunnelSoft.MiniScript.YSL.Test;
using TunnelSoft.YSL.Features.CodeGenerator;
using TunnelSoft.YSL.Features.CodeGenerator.Interfaces;

public class OptimizationTests {
    private JQueryCodeGenerator generator;

    public OptimizationTests() {

    }

    [TestInitialize]
    public void Initialize() {
        //var config = new MiniScriptConfiguration { EnableOptimizations = true };
        //generator = new JQueryCodeGenerator(config);
    }


    [TestMethod]
    public async Task TestOptimization_LoopInvariantCodeMotion() {
        var input = @"
            var x = 5;
            for (var i = 0; i < 10; i = i + 1) {
                var y = x * 2;
                print(y);
            }";
        var result = await generator.GenerateCode(input);
        Assert.IsTrue(result.Data.IsCompileSuccess);
        Debug.Contains("var y = x * 2;", result.Data.CompiledCode);
        Debug.Contains("for (var i = 0; i < 10; i = i + 1) {", result.Data.CompiledCode);
        Debug.DoesNotContain("var y = x * 2;", result.Data.CompiledCode.Substring(result.Data.CompiledCode.IndexOf("for")));
    }
}
