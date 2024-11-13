using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using TunnelSoft.Utilities;
using TunnelSoft.Utilities.Result;
using TunnelSoft.YSL.Features.CodeGenerator.Interfaces;
using static TunnelSoft.YSL.Features.CodeGenerator.Constants;
using static TunnelSoft.Utilities.Utilities;

namespace TunnelSoft.YSL.Features.CodeGenerator;

public class JQueryCodeGenerator : ICodeGenerator {


    private readonly CompilerSettings? compilerSettings;
    private readonly string CompileWritePath;
    private const string NO_RESULT_FOUND = "No errors found.";

    public JQueryCodeGenerator() {

    }

    public JQueryCodeGenerator(CompilerSettings? compilerSettings) {
        this.compilerSettings = compilerSettings;
        var tempPath = compilerSettings.CompileWritePath;
        CompileWritePath = tempPath.TrimEnd("\\") + "\\";
    }

    public string Statistics { get; set; }
    public string GetStatistics() {
        return Statistics;
    }


    public async Task<ServiceResult<Compile_Response>> GenerateCode(string input, string? fileName = null) {
        // Create a stream from the input
        AntlrInputStream inputStream = new AntlrInputStream(input);

        // Create a lexer that feeds off of input stream
        MiniScriptLexer lexer = new MiniScriptLexer(inputStream);

        // Create a buffer of tokens pulled from the lexer
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

        // Create a parser that feeds off the tokens
        MiniScriptParser parser = new MiniScriptParser(commonTokenStream);

        //var treeResult = DebuggerVisualizerAttribute

        var resultMessage = "";
        try {
            // Use the visitor to generate jQuery code
            var visitor = new JQueryCodeGeneratorVisitor();
            var compileResult4 = visitor.CompileParserCode(parser);
            
            var visualTree = visitor.VisualizeTree(0);
            if (!CompileWritePath.IsEmpty() && !fileName.IsEmpty()) {
                EnsureDirectory(CompileWritePath);
                //we are going to write file execution result
                string fileResult = "//Created @" + DateTime.Now.ToString() + Environment.NewLine;
                fileResult += "//Compiler Version is " + Ysl_Version + Environment.NewLine;
                fileResult += compileResult4;
                if (File.Exists(CompileWritePath + fileName)) {
                    File.Delete(CompileWritePath + fileName);
                }
                File.WriteAllText(CompileWritePath + fileName, fileResult);
            }

            // Generate jQuery code from the parsed context
            //var t1 = ConvertScriptToJQuery(scriptContext);

            var CompileErrors = visitor.GetErrors();
            var IsCompileSuccess = CompileErrors == NO_RESULT_FOUND;

            var t2 = new Compile_Response() {
                CompiledCode = compileResult4,
                Statistics = visitor.GetStatistics(),
                CompileErrors = CompileErrors,
                IsCompileSuccess = IsCompileSuccess,
                VisualTree = visualTree,
            };

            return ServiceResult<Compile_Response>.SuccessResult(t2);

        } catch (MiniScriptParseException ex) {
            resultMessage = "MiniScriptParseException: " + ex.Message;
            //errors.Add(new CompilationError(context.Start.Line, context.Start.Column, ex.Message));
        } catch (Exception ex) {
            resultMessage += ex.Message;
            if (ex.InnerException != null) {
                resultMessage += Environment.NewLine + ex.InnerException.Message;
            }
        }

        return ServiceResult<Compile_Response>.FailureResult(resultMessage);

    }


}