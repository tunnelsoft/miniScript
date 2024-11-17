﻿namespace TunnelSoft.MiniScript.YSL.Test;
using TunnelSoft.YSL.Features.CodeGenerator;

public class StandardLibraryTests {
    private JQueryCodeGenerator generator;


    [TestInitialize]
    public void Initialize() {
        generator = new JQueryCodeGenerator();
    }


    //public StandardLibraryTests() {
    //    generator = new JQueryCodeGenerator();
    //}

    [TestMethod]
    public async Task TestStandardLibrary_Print() {
        var result = await generator.GenerateCode("print('Hello, World!');");
        Assert.IsTrue(result.IsSuccess);
        Assert.Contains("console.log('Hello, World!');", result.CompiledCode);
    }

    [TestMethod]
    public async Task TestStandardLibrary_MathOperations() {
        var result = await generator.GenerateCode("var x = Math.max(5, 10);");
        Assert.IsTrue(result.IsSuccess);
        Assert.Contains("var x = Math.max(5, 10);", result.CompiledCode);
    }
}