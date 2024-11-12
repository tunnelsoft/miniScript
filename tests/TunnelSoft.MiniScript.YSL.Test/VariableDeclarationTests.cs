using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TunnelSoft.YSL.Features.CodeGenerator.Interfaces;
using TunnelSoft.YSL.Features.CodeGenerator;
using TunnelSoft.YSL;


namespace TunnelSoft.MiniScript.YSL.Test;

[TestClass]
public class CodeGeneratorTests {

    private const string Expected_Invalid_Error = @"Compilation Errors:
Error at line 1, column 7, Symbol `invalid`, Message: no viable alternative at input 'helloinvalid'
1:1 - Unrecognized statement
";

    private ICodeGenerator codeGenerator;

    [TestInitialize]
    public void Initialize() {
        codeGenerator = new JQueryCodeGenerator(); // Replace with the actual implementation
    }

    [TestMethod]
    public async Task GenerateCode_ValidInput_ReturnsCompiledCode() {
        // Arrange
        var input = "var t1 = 0;";
        var expectedOutput = "    var t1 = 0;";

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsTrue(result.Data.IsCompileSuccess);
        Assert.IsTrue(result.Data.CompileErrors == Constants.NO_ERROR_FOUND);

        Assert.AreEqual(expectedOutput, result.Data.CompiledCode);
    }

    [TestMethod]
    //[ExpectedException(typeof(MiniScriptParseException), "miniscript does not recognize any code.")]
    public async Task GenerateCode_InvalidInput_ReturnsError() {
        // Arrange
        var input = " hello invalid code";

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success, "miniscript tried to compile, but there are errors");
        Assert.IsFalse(result.Data.IsCompileSuccess, "miniscript compiler found errors");
        Assert.AreEqual(Expected_Invalid_Error, result.Data.CompileErrors);
    }

    [TestMethod]
    public async Task GenerateCode_InputVariable_ReturnsCompiledCode() {
        // Arrange
        var input = "input1 = '100';";
        var expectedOutput = "$('#input1').val('100');";

        // Act
        var result = await codeGenerator.GenerateCode(input);
        var code = result.Data.CompiledCode.Trim();
        // Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual(expectedOutput, code);
    }

    [TestMethod]
    public async Task GenerateCode_ComplexInput_ReturnsCompiledCode() {
        // Arrange
        var input = "var t1 = 0; input1 = '100';";
        var expectedOutput = @"    var t1 = 0;
    $('#input1').val('100');";

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual(expectedOutput, result.Data.CompiledCode);
    }
}