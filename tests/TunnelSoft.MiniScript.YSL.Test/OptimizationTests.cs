namespace TunnelSoft.MiniScript.YSL.Test;
using TunnelSoft.YSL.Features.CodeGenerator;

public class OptimizationTests {
    private JQueryCodeGenerator generator;

    public OptimizationTests() {
        var config = new MiniScriptConfiguration { EnableOptimizations = true };
        generator = new JQueryCodeGenerator(config);
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
        Assert.IsTrue(result.IsSuccess);
        Assert.Contains("var y = x * 2;", result.CompiledCode);
        Assert.Contains("for (var i = 0; i < 10; i = i + 1) {", result.CompiledCode);
        Assert.DoesNotContain("var y = x * 2;", result.CompiledCode.Substring(result.CompiledCode.IndexOf("for")));
    }
}
