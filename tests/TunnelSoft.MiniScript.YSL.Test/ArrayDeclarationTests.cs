using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TunnelSoft.YSL.Features.CodeGenerator.Interfaces;
using TunnelSoft.YSL.Features.CodeGenerator;
using TunnelSoft.YSL;

namespace TunnelSoft.MiniScript.YSL.Test;


[TestClass]
public class ArrayDeclarationTests {
    private const string EMPTY_ARRAY_INPUT = "var numbers = [];";
    private const string EMPTY_ARRAY_EXPECTED_OUTPUT = "    var numbers = [];";

    private const string SINGLE_ARRAY_INPUT = "var numbers2 = [1, 2, 3];";
    private const string SINGLE_ARRAY_EXPECTED_OUTPUT = "    var numbers2 = [1, 2, 3];";

    private const string MULTIPLE_ARRAY_INPUT = "var numbers3 = [1, 2, 3, 4, 5];";
    private const string MULTIPLE_ARRAY_EXPECTED_OUTPUT = "    var numbers3 = [1, 2, 3, 4, 5];";

    private const string NESTED_ARRAY_INPUT = "var numbers4 = [[]];";
    private const string NESTED_ARRAY_EXPECTED_OUTPUT = "    var numbers4 = [[]];";

    private const string INVALID_ARRAY_INPUT_NO_END = "var numbers5 = [;";
    private const string INVALID_ARRAY_INPUT_NO_START = "var numbers6 = ];";

    private const string MISSING_CLOSING_BRACKET_INPUT = "var numbers7 = [1, 2, 3";
    private const string MISSING_OPENING_BRACKET_INPUT = "var numbers8 = 1, 2, 3];";

    private ICodeGenerator codeGenerator;

    [TestInitialize]
    public void Initialize() {
        codeGenerator = new JQueryCodeGenerator();
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_EmptyArray_ReturnsSuccess() {
        // Arrange
        var input = EMPTY_ARRAY_INPUT;
        var expectedOutput = EMPTY_ARRAY_EXPECTED_OUTPUT;

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsTrue(result.Data.IsCompileSuccess);
        Assert.IsTrue(result.Data.CompileErrors == Constants.NO_ERROR_FOUND);
        Assert.AreEqual(expectedOutput, result.Data.CompiledCode);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_SingleArray_ReturnsSuccess() {
        // Arrange
        var input = SINGLE_ARRAY_INPUT;
        var expectedOutput = SINGLE_ARRAY_EXPECTED_OUTPUT;

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsTrue(result.Data.IsCompileSuccess);
        Assert.IsTrue(result.Data.CompileErrors == Constants.NO_ERROR_FOUND);
        Assert.AreEqual(expectedOutput, result.Data.CompiledCode);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_MultipleArray_ReturnsSuccess() {
        // Arrange
        var input = MULTIPLE_ARRAY_INPUT;
        var expectedOutput = MULTIPLE_ARRAY_EXPECTED_OUTPUT;

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsTrue(result.Data.IsCompileSuccess);
        Assert.IsTrue(result.Data.CompileErrors == Constants.NO_ERROR_FOUND);
        Assert.AreEqual(expectedOutput, result.Data.CompiledCode);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_NestedArray_ReturnsSuccess() {
        // Arrange
        var input = NESTED_ARRAY_INPUT;
        var expectedOutput = NESTED_ARRAY_EXPECTED_OUTPUT;

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsTrue(result.Data.IsCompileSuccess);
        Assert.IsTrue(result.Data.CompileErrors == Constants.NO_ERROR_FOUND);
        Assert.AreEqual(expectedOutput, result.Data.CompiledCode);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_InvalidArray_ReturnsError() {
        // Arrange
        var input = INVALID_ARRAY_INPUT_NO_END;

        var expectedMsg = @"Compilation Errors:
Error at line 1, column 16, Symbol `;`, Message: mismatched input ';' expecting {'not', '!', '+', '-', '(', '[', ']', STRING, FLOAT, NUMBER, BOOLEAN, IDENTIFIER}
1:15 - Invalid Bracket (start or end)
";
        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success, "miniscript tried to compile, but there are errors");
        Assert.IsFalse(result.Data.IsCompileSuccess, "miniscript compiler found errors");
        Assert.AreNotEqual(Constants.NO_ERROR_FOUND, result.Data.CompileErrors);
        Assert.AreEqual(expectedMsg, result.Data.CompileErrors);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_InvalidArray2_ReturnsError() {
        // Arrange
        var input = INVALID_ARRAY_INPUT_NO_START;
        var expectedError = @"Compilation Errors:
Error at line 1, column 15, Symbol `]`, Message: mismatched input ']' expecting {'not', '!', '+', '-', '(', '[', STRING, FLOAT, NUMBER, BOOLEAN, IDENTIFIER}
1:15 - Unrecognized statement
";

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success, "miniscript tried to compile, but there are errors");
        Assert.IsFalse(result.Data.IsCompileSuccess, "miniscript compiler found errors");
        Assert.AreNotEqual(Constants.NO_ERROR_FOUND, result.Data.CompileErrors);
        Assert.AreEqual(expectedError, result.Data.CompileErrors);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_MissingClosingBracket_ReturnsError() {
        // Arrange
        var input = MISSING_CLOSING_BRACKET_INPUT;
        var expectedError = @"Compilation Errors:
Error at line 1, column 23, Symbol `<EOF>`, Message: extraneous input '<EOF>' expecting {',', ']'}
1:15 - Invalid Bracket (start or end)
";
        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success, "miniscript tried to compile, but there are errors");
        Assert.IsFalse(result.Data.IsCompileSuccess, "miniscript compiler found errors");
        Assert.AreNotEqual(Constants.NO_ERROR_FOUND, result.Data.CompileErrors);
        Assert.AreEqual(expectedError, result.Data.CompileErrors);
    }

    [TestMethod]
    public async Task Compile_ArrayDeclaration_MissingOpeningBracket_ReturnsError() {
        // Arrange
        var input = MISSING_OPENING_BRACKET_INPUT;

        // Act
        var result = await codeGenerator.GenerateCode(input);

        // Assert
        Assert.IsTrue(result.Success, "miniscript tried to compile, but there are errors");
        Assert.IsFalse(result.Data.IsCompileSuccess, "miniscript compiler found errors");
        Assert.AreNotEqual(Constants.NO_ERROR_FOUND, result.Data.CompileErrors);
    }
}