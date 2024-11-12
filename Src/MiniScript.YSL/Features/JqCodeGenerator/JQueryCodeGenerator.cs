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





    #region Solution 3

    //static string GenerateJQuery(MiniScriptParser.ProgramContext programContext) {
    //    StringBuilder jqueryBuilder = new StringBuilder();

    //    foreach (var statement in programContext.statement()) {
    //        if (statement.assignment() != null) {
    //            var assignmentContext = statement.assignment();
    //            var variable = assignmentContext.variable().GetText();
    //            var expression = assignmentContext.expression();

    //            // Convert the variable to jQuery selector syntax
    //            string jqueryVariable = $"$('#{variable}')";

    //            // Generate the expression in jQuery syntax
    //            string jqueryExpression = ConvertExpressionToJQuery2(expression);

    //            // Generate the final jQuery assignment
    //            jqueryBuilder.AppendLine($"{jqueryVariable}.val({jqueryExpression});");
    //        } else if (statement.ifStatement() != null) {
    //            jqueryBuilder.AppendLine(ConvertIfStatementToJQuery2(statement.ifStatement()));
    //        } else if (statement.loopStatement() != null) {
    //            jqueryBuilder.AppendLine(ConvertLoopStatementToJQuery2(statement.loopStatement()));
    //            //} else if (statement.functionCall() != null) {
    //            //    jqueryBuilder.AppendLine(ConvertFunctionCallToJQuery(statement.functionCall()));
    //        } else {
    //            string jqueryExpression = ConvertExpressionToJQuery2(statement.expression());
    //            jqueryBuilder.AppendLine(jqueryExpression);
    //        }
    //    }

    //    return jqueryBuilder.ToString();
    //}

    //static string ConvertIfStatementToJQuery2(MiniScriptParser.IfStatementContext ifStatementContext) {
    //    StringBuilder ifBuilder = new StringBuilder();
    //    ifBuilder.Append("if (");
    //    ifBuilder.Append(ConvertExpressionToJQuery2(ifStatementContext.expression()));
    //    ifBuilder.AppendLine(") {");

    //    foreach (var stmt in ifStatementContext.statement()) {
    //        if (stmt.assignment() != null) {
    //            var assignmentContext = stmt.assignment();
    //            var variable = assignmentContext.variable().GetText();
    //            var expression = assignmentContext.expression();
    //            string jqueryVariable = $"$('#{variable}')";
    //            string jqueryExpression = ConvertExpressionToJQuery2(expression);
    //            ifBuilder.AppendLine($"    {jqueryVariable}.val({jqueryExpression});");
    //        }
    //    }

    //    ifBuilder.AppendLine("}");

    //    if (ifStatementContext.ELSE() != null) {
    //        ifBuilder.AppendLine("else {");
    //        foreach (var stmt in ifStatementContext.statement()) {
    //            if (stmt.assignment() != null) {
    //                var assignmentContext = stmt.assignment();
    //                var variable = assignmentContext.variable().GetText();
    //                var expression = assignmentContext.expression();
    //                string jqueryVariable = $"$('#{variable}')";
    //                string jqueryExpression = ConvertExpressionToJQuery2(expression);
    //                ifBuilder.AppendLine($"    {jqueryVariable}.val({jqueryExpression});");
    //            }
    //        }
    //        ifBuilder.AppendLine("}");
    //    }

    //    return ifBuilder.ToString();
    //}

    //static string ConvertLoopStatementToJQuery2(MiniScriptParser.LoopStatementContext loopStatementContext) {
    //    StringBuilder loopBuilder = new StringBuilder();
    //    loopBuilder.AppendLine("while (true) {"); // Assuming a simple infinite loop for demonstration
    //    foreach (var stmt in loopStatementContext.statement()) {
    //        if (stmt.assignment() != null) {
    //            var assignmentContext = stmt.assignment();
    //            var variable = assignmentContext.variable().GetText();
    //            var expression = assignmentContext.expression();
    //            string jqueryVariable = $"$('#{variable}')";
    //            string jqueryExpression = ConvertExpressionToJQuery2(expression);
    //            loopBuilder.AppendLine($"    {jqueryVariable}.val({jqueryExpression});");
    //        }
    //    }
    //    loopBuilder.AppendLine("}");
    //    return loopBuilder.ToString();
    //}

    //static string ConvertFunctionCallToJQuery(MiniScriptParser.FunctionCallContext functionCallContext) {
    //    StringBuilder argsBuilder = new StringBuilder();
    //    for (int i = 0; i < functionCallContext.expression().Length; i++) {
    //        var arg = functionCallContext.expression(i);
    //        if (i > 0) {
    //            argsBuilder.Append(", "); // Add comma between arguments
    //        }
    //        argsBuilder.Append(ConvertExpressionToJQuery2(arg)); // Convert each argument
    //    }

    //    return $"{functionCallContext.FUNC().GetText()}({argsBuilder});"; // Generate the function call
    //}

    //static string ConvertExpressionToJQuery2(MiniScriptParser.ExpressionContext expressionContext) {
    //    StringBuilder expressionBuilder = new StringBuilder();

    //    if (expressionContext.term().Length > 0) {
    //        for (int i = 0; i < expressionContext.term().Length; i++) {
    //            var termContext = expressionContext.term(i);
    //            if (i > 0) {
    //                // Add operator between terms
    //                if (expressionContext.PLUS().Length > i - 1) {
    //                    expressionBuilder.Append(" + ");
    //                } else if (expressionContext.MINUS().Length > i - 1) {
    //                    expressionBuilder.Append(" - ");
    //                }
    //            }

    //            // Process each term
    //            expressionBuilder.Append(ConvertTermToJQuery(termContext));
    //        }
    //    }

    //    return expressionBuilder.ToString();
    //}

    //static string ConvertTermToJQuery(MiniScriptParser.TermContext termContext) {
    //    StringBuilder termBuilder = new StringBuilder();

    //    if (termContext.factor().Length > 0) {
    //        for (int i = 0; i < termContext.factor().Length; i++) {
    //            var factorContext = termContext.factor(i);
    //            if (i > 0) {
    //                // Add operator between factors
    //                if (termContext.MUL().Length > i - 1) {
    //                    termBuilder.Append(" * ");
    //                } else if (termContext.DIV().Length > i - 1) {
    //                    termBuilder.Append(" / ");
    //                }
    //            }

    //            // Process each factor
    //            termBuilder.Append(ConvertFactorToJQuery(factorContext));
    //        }
    //    }

    //    return termBuilder.ToString();
    //}

    //static string ConvertFactorToJQuery(MiniScriptParser.FactorContext factorContext) {
    //    if (factorContext.NUMBER() != null) {
    //        return factorContext.NUMBER().GetText(); // Return numbers as-is
    //    } else if (factorContext.STRING() != null) // Handle string literals
    //      {
    //        return factorContext.STRING().GetText(); // Return string literals with quotes
    //    } else if (factorContext.variable() != null) {
    //        // Convert the variable to jQuery selector syntax
    //        return $"$('#{factorContext.variable().GetText()}').val()";
    //    } else if (factorContext.functionCall() != null) {
    //        // Handle function calls
    //        return ConvertFunctionCallToJQuery(factorContext.functionCall());
    //    } else if (factorContext.LPAREN() != null) {
    //        // Handle parentheses
    //        return $"({ConvertExpressionToJQuery2(factorContext.expression())})";
    //    }

    //    return string.Empty;
    //}

    #endregion


}


