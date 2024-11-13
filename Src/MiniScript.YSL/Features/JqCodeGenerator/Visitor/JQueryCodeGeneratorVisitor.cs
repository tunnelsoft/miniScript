using System.Collections.Generic;
using System.Text;
using static TunnelSoft.YSL.Features.CodeGenerator.Constants;
using Antlr4.Runtime;
using Serilog;
using TunnelSoft.Utilities;
using TunnelSoft.MiniScript.YSL.Symbols;
using TunnelSoft.MiniScript.YSL.Symbols.Base;


namespace TunnelSoft.YSL.Features.CodeGenerator;

public partial class JQueryCodeGeneratorVisitor : MiniScriptBaseVisitor<string> {
    // Symbol Table to manage scopes and declarations
    private readonly SymbolTable symbolTable = new SymbolTable();

    // HashSet to track declared inputs (tied to DOM elements)
    private readonly HashSet<string> declaredInputs = new HashSet<string>();

    // StringBuilder for the generated JavaScript code
    private readonly StringBuilder jqueryBuilder = new StringBuilder();

    // Stack to manage loop labels for nested loops
    private readonly Stack<string> loopLabels = new Stack<string>();
    private int loopCounter = 0;





    //to get errors from right place.
    private MiniScriptErrorListener errorListener = new MiniScriptErrorListener();
    private MiniScriptParser? _parser = null;
    MiniScriptParser.ProgramContext? _tree = null;

    public string CompileParserCode(MiniScriptParser miniScriptParser) {
        _parser = miniScriptParser;
        _parser.AddErrorListener(errorListener);

        // Start parsing at the entry point of the grammar
        _tree = _parser.program();
        var compileResult4 = Visit(_tree);
        return compileResult4;
    }


    // Method to retrieve the generated JavaScript code
    public string ReturnCode() {
        return jqueryBuilder.ToString();
    }


    public string GetStatistics() {
        var results = new StringBuilder();
        results.AppendLine("Statistics: ");
        // results.AppendLine(string.Format("{0, 15} | {1}", "Declarations", declaredVariables.Count));
        results.AppendLine(string.Format("{0, 15} | {1}", "Inputs", declaredInputs.Count));
        return results.ToString();
    }

    public override string VisitStatement(MiniScriptParser.StatementContext context) {
        if (context.variableDeclaration() != null)
            return Visit(context.variableDeclaration());
        else if (context.assignment() != null)
            return Visit(context.assignment());
        else if (context.ifStatement() != null)
            return Visit(context.ifStatement());
        else if (context.loopStatement() != null)
            return Visit(context.loopStatement());
        else if (context.forStatement() != null)
            return Visit(context.forStatement());
        else if (context.breakStatement() != null)
            return Visit(context.breakStatement());
        else if (context.continueStatement() != null)
            return Visit(context.continueStatement());
        else if (context.expressionStatement() != null)
            return Visit(context.expressionStatement());
        else if (context.functionCallStatement() != null)
            return Visit(context.functionCallStatement());
        else if (context.comment() != null)
            return Visit(context.comment());
        else if (context.multilineComment() != null)
            return Visit(context.multilineComment());
        else if (context.multilineComment() != null)
            return Visit(context.multilineComment());

        else
            throw new MiniScriptParseException($"Unrecognized statement", context.Start.Line, context.Start.Column, context.GetText());
    }


    // Override VisitAssignment
    public override string VisitAssignment(MiniScriptParser.AssignmentContext context) {
        string assignmentCode = string.Empty;
        var line = context.Start.Line;
        var column = context.Start.Column;

        if (context.IDENTIFIER() != null) {
            var variable = context.IDENTIFIER().GetText();

            if (symbolTable.IsDeclared(variable)) {
                var symbol = symbolTable.GetSymbol(variable);
                var expression = Visit(context.expression());

                if (symbol.Type == SymbolType.Variable) {
                    assignmentCode = $"{variable} = {expression};";
                } else if (symbol.Type == SymbolType.Input) {
                    // Assigning to input via jQuery
                    assignmentCode = $"$('#{variable}').val({expression});";
                }
            } else {
                // Treat undeclared identifier as an input tied to DOM element
                declaredInputs.Add(variable);
                symbolTable.Declare(variable, SymbolType.Input);
                var expression = Visit(context.expression());
                assignmentCode = $"$('#{variable}').val({expression});";
            }
        } else if (context.arrayAccess() != null) {
            var arrayAccess = Visit(context.arrayAccess());
            var expression = Visit(context.expression());
            assignmentCode = $"{arrayAccess} = {expression};";
        } else {
            compileErrors.Add(new CompilationError(line, column, "Invalid assignment target.", context.GetText()));
        }

        return assignmentCode;
    }

    // Override VisitIfStatement
    public override string VisitIfStatement(MiniScriptParser.IfStatementContext context) {
        var condition = Visit(context.expression());
        var line = context.Start.Line;
        var column = context.Start.Column;

        StringBuilder ifBuilder = new StringBuilder();
        ifBuilder.AppendLine($"if ({condition}) {{");
        symbolTable.EnterScope(); // Enter new scope for if block

        foreach (var stmt in context.block(0).statement()) {
            var stmtCode = Visit(stmt);
            if (!string.IsNullOrWhiteSpace(stmtCode))
                ifBuilder.AppendLine("    " + stmtCode);
        }

        symbolTable.ExitScope(); // Exit if block scope
        ifBuilder.AppendLine("}");

        if (context.ELSE() != null && context.block().Length > 1) {
            ifBuilder.AppendLine("else {");
            symbolTable.EnterScope(); // Enter new scope for else block

            foreach (var stmt in context.block(1).statement()) {
                var stmtCode = Visit(stmt);
                if (!string.IsNullOrWhiteSpace(stmtCode))
                    ifBuilder.AppendLine("    " + stmtCode);
            }

            symbolTable.ExitScope(); // Exit else block scope
            ifBuilder.AppendLine("}");
        }

        return ifBuilder.ToString();
    }

    // Override VisitLoopStatement (assuming 'loop' maps to 'while')
    public override string VisitLoopStatement(MiniScriptParser.LoopStatementContext context) {
        var condition = Visit(context.expression());
        var line = context.Start.Line;
        var column = context.Start.Column;

        StringBuilder loopBuilder = new StringBuilder();
        loopBuilder.AppendLine($"while ({condition}) {{");
        symbolTable.EnterScope(); // Enter loop scope

        foreach (var stmt in context.block().statement()) {
            var stmtCode = Visit(stmt);
            if (!string.IsNullOrWhiteSpace(stmtCode))
                loopBuilder.AppendLine("    " + stmtCode);
        }

        symbolTable.ExitScope(); // Exit loop scope
        loopBuilder.AppendLine("}");
        return loopBuilder.ToString();
    }

    // Override VisitForStatement
    public override string VisitForStatement(MiniScriptParser.ForStatementContext context) {
        // Extract initialization, condition, and iteration expressions
        string initialization = context.variableDeclaration() != null
                                ? Visit(context.variableDeclaration()).TrimEnd(';')
                                : (context.assignment().Length > 0
                                    ? Visit(context.assignment(0)).TrimEnd(';')
                                    : string.Empty);

        string condition = context.expression() != null
                            ? Visit(context.expression())
                            : string.Empty;

        string iteration = context.assignment().Length > 1
                            ? Visit(context.assignment(1)).TrimEnd(';')
                            : string.Empty;

        var line = context.Start.Line;
        var column = context.Start.Column;

        StringBuilder forBuilder = new StringBuilder();

        forBuilder.Append($"for ({initialization}; {condition}; {iteration}) {{");

        // Handle loop labels for nested loops
        string label = $"loop{loopCounter++}";
        loopLabels.Push(label);
        forBuilder.AppendLine($" {label}: {{"); // Add a label for the loop

        symbolTable.EnterScope(); // Enter loop block scope

        foreach (var stmt in context.block().statement()) {
            var stmtCode = Visit(stmt);
            if (!string.IsNullOrWhiteSpace(stmtCode))
                forBuilder.AppendLine("    " + stmtCode);
        }

        symbolTable.ExitScope(); // Exit loop block scope
        forBuilder.AppendLine("}"); // Close labeled block
        forBuilder.AppendLine("}"); // Close for loop

        loopLabels.Pop();

        return forBuilder.ToString();
    }

    // Override VisitBreakStatement
    public override string VisitBreakStatement(MiniScriptParser.BreakStatementContext context) {
        var line = context.Start.Line;
        var column = context.Start.Column;

        if (loopLabels.Count > 0) {
            var currentLabel = loopLabels.Peek();
            return $"break {currentLabel};";
        } else {
            compileErrors.Add(new CompilationError(line, column, "Break statement used outside of a loop.", context.GetText()));
            return string.Empty;
        }
    }

    // Override VisitContinueStatement
    public override string VisitContinueStatement(MiniScriptParser.ContinueStatementContext context) {
        var line = context.Start.Line;
        var column = context.Start.Column;

        if (loopLabels.Count > 0) {
            var currentLabel = loopLabels.Peek();
            return $"continue {currentLabel};";
        } else {
            compileErrors.Add(new CompilationError(line, column, "Continue statement used outside of a loop.", context.GetText()));
            return string.Empty;
        }
    }


    public override string VisitExpression(MiniScriptParser.ExpressionContext context) {
        return Visit(context.comparisonExpression());
    }

    // Override VisitComparisonExpression
    public override string VisitComparisonExpression(MiniScriptParser.ComparisonExpressionContext context) {
        if (context.additiveExpression().Length == 1) {
            // No comparison operator, return the additive expression
            return Visit(context.additiveExpression(0));
        }

        var left = Visit(context.additiveExpression(0));
        var operatorSymbol = context.GetChild(1).GetText(); // e.g., '>', '<', '==', etc.
        var right = Visit(context.additiveExpression(1));

        return $"{left} {operatorSymbol} {right}";
    }

    // Override VisitAdditiveExpression
    public override string VisitAdditiveExpression(MiniScriptParser.AdditiveExpressionContext context) {
        if (context.multiplicativeExpression().Length == 1) {
            // Single multiplicative expression
            return Visit(context.multiplicativeExpression(0));
        }

        StringBuilder exprBuilder = new StringBuilder();

        for (int i = 0; i < context.multiplicativeExpression().Length; i++) {
            if (i > 0) {
                var operatorSymbol = context.GetChild(2 * i - 1).GetText(); // '+', '-'
                exprBuilder.Append($" {operatorSymbol} ");
            }

            exprBuilder.Append(Visit(context.multiplicativeExpression(i)));
        }

        return exprBuilder.ToString();
    }

    // Override VisitMultiplicativeExpression
    public override string VisitMultiplicativeExpression(MiniScriptParser.MultiplicativeExpressionContext context) {
        if (context.powerExpression().Length == 1) {
            // Single power expression
            return Visit(context.powerExpression(0));
        }

        StringBuilder exprBuilder = new StringBuilder();

        for (int i = 0; i < context.powerExpression().Length; i++) {
            if (i > 0) {
                var operatorSymbol = context.GetChild(2 * i - 1).GetText(); // '*', '/', '%'
                exprBuilder.Append($" {operatorSymbol} ");
            }

            exprBuilder.Append(Visit(context.powerExpression(i)));
        }

        return exprBuilder.ToString();
    }

    // Override VisitPowerExpression
    public override string VisitPowerExpression(MiniScriptParser.PowerExpressionContext context) {
        // Check if the POW operator is present, indicating a recursive power expression
        if (context.POW() != null && context.powerExpression() != null) {
            var baseExpr = Visit(context.unaryExpression());
            var exponent = Visit(context.powerExpression());
            return $"{baseExpr} ** {exponent}"; // Using ** for exponentiation in JS
        } else {
            // Simple power expression without exponentiation
            return Visit(context.unaryExpression());
        }
    }

    // Override VisitUnaryExpression
    public override string VisitUnaryExpression(MiniScriptParser.UnaryExpressionContext context) {
        if (context.GetChild(0).GetText() == "not" || context.GetChild(0).GetText() == "!") {
            var operatorSymbol = context.GetChild(0).GetText() == "not" ? "!" : "!";
            var operand = Visit(context.unaryExpression());
            return $"{operatorSymbol}({operand})";
        } else if (context.GetChild(0).GetText() == "+") {
            var operand = Visit(context.unaryExpression());
            return $"+({operand})";
        } else if (context.GetChild(0).GetText() == "-") {
            var operand = Visit(context.unaryExpression());
            return $"-({operand})";
        }

        if (context.primaryExpression() is not null) {
            return Visit(context.primaryExpression());
        }

        //for now, i am going to check invalid input right here
        var txt = context.GetText();
        if (txt == "[" || txt == "]") {
            throw new MiniScriptParseException($"Unrecognized statement", context.Start.Line, context.Start.Column, context.GetText());
        }
        return string.Empty;
    }

    // Override VisitPrimaryExpression
    public override string VisitPrimaryExpression(MiniScriptParser.PrimaryExpressionContext context) {
        if (context.NUMBER() != null) {
            return context.NUMBER().GetText();
        } else if (context.FLOAT() != null) {
            return context.FLOAT().GetText();
        } else if (context.BOOLEAN() != null) {
            return context.BOOLEAN().GetText().ToLower(); // Convert to lowercase (true/false)
        } else if (context.STRING() != null) {
            return context.STRING().GetText();
        } else if (context.functionCall() != null) {
            return Visit(context.functionCall());
        } else if (context.arrayAccess() != null) {
            return Visit(context.arrayAccess());
        } else if (context.IDENTIFIER() != null) {
            var identifier = context.IDENTIFIER().GetText();
            var symbol = symbolTable.GetSymbol(identifier);
            if (symbol != null) {
                if (symbol.Type == SymbolType.Variable)
                    return identifier;
                else if (symbol.Type == SymbolType.Input)
                    return $"$('#{identifier}').val()";
            } else {
                // Undeclared identifier treated as input
                declaredInputs.Add(identifier);
                symbolTable.Declare(identifier, SymbolType.Input);
                return $"$('#{identifier}').val()";
            }
        } else if (context.expression() != null) {
            return $"({Visit(context.expression())})";
        } else if (context.arrayLiteral() != null) {
            return Visit(context.arrayLiteral());
        }

        return string.Empty;
    }

    // Override VisitFunctionCall
    public override string VisitFunctionCall(MiniScriptParser.FunctionCallContext context) {
        var functionName = Visit(context.qualifiedIdentifier());
        StringBuilder argsBuilder = new StringBuilder();

        for (int i = 0; i < context.expression().Length; i++) {
            if (i > 0)
                argsBuilder.Append(", ");
            argsBuilder.Append(Visit(context.expression(i)));
        }

        return $"{functionName}({argsBuilder});";
    }

    // Override VisitQualifiedIdentifier
    public override string VisitQualifiedIdentifier(MiniScriptParser.QualifiedIdentifierContext context) {
        StringBuilder qualifiedName = new StringBuilder();

        for (int i = 0; i < context.IDENTIFIER().Length; i++) {
            if (i > 0)
                qualifiedName.Append(".");
            qualifiedName.Append(context.IDENTIFIER(i).GetText());
        }

        return qualifiedName.ToString();
    }

    // Override VisitArrayLiteral
    public override string VisitArrayLiteral(MiniScriptParser.ArrayLiteralContext context) {
        if (context.LBRACKET() is null || context.RBRACKET() is null) {
            throw new MiniScriptParseException($"Invalid Bracket (start or end)", context.Start.Line, context.Start.Column, context.GetText());
        }
        StringBuilder arrayBuilder = new StringBuilder("[");
        for (int i = 0; i < context.expression().Length; i++) {
            if (i > 0)
                arrayBuilder.Append(", ");
            arrayBuilder.Append(Visit(context.expression(i)));
        }
        arrayBuilder.Append("]");
        return arrayBuilder.ToString();
    }

    // Override VisitArrayAccess
    public override string VisitArrayAccess(MiniScriptParser.ArrayAccessContext context) {
        var arrayName = context.IDENTIFIER().GetText();
        var index = Visit(context.expression());
        return $"{arrayName}[{index}]";
    }

    // Override VisitComment to preserve single-line comments
    public override string VisitComment(MiniScriptParser.CommentContext context) {
        var commentText = context.GetText();
        return $"{commentText}";
    }

    // Override VisitMultilineComment to preserve multi-line comments
    public override string VisitMultilineComment(MiniScriptParser.MultilineCommentContext context) {
        var commentText = context.GetText();
        return $"/*{commentText}*/";
    }

    // Override VisitError for catch-all or specific error contexts
    public override string VisitError(MiniScriptParser.ErrorContext context) {
        var errorText = context.GetText();
        var line = context.Start.Line;
        var column = context.Start.Column;
        compileErrors.Add(new CompilationError(line, column, $"Unexpected token: {errorText}", context.GetText()));
        return string.Empty;
    }

    // Additional Overrides for other constructs as needed
}