using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using TunnelSoft.MiniScript.YSL.Symbols;
using TunnelSoft.MiniScript.YSL.Symbols.Base;

//using TunnelSoft.YSL.Features.CodeGenerator.SymbolTable; 

namespace TunnelSoft.YSL.Features.CodeGenerator;
public partial class JQueryCodeGeneratorVisitor  {

    // Override VisitVariableDeclaration
    public override string VisitVariableDeclaration(MiniScriptParser.VariableDeclarationContext context) {
        var variable = context.IDENTIFIER().GetText();
        var line = context.Start.Line;
        var column = context.Start.Column;

        // Check for redefinition in the current scope
        if (symbolTable.IsDeclaredInCurrentScope(variable)) {
            compileErrors.Add(new CompilationError(line, column, $"Redefinition of variable '{variable}' detected.", context.GetText()));
            return string.Empty;
        }

        // Add the variable to the symbol table
        symbolTable.Declare(variable, SymbolType.Variable);

        // Handle optional initialization
        if (context.expression() != null) {

            try {
                var expression = Visit(context.expression());
                return $"var {variable} = {expression};";
            } catch (MiniScriptParseException ex) {

                var e2 = new CompilationError(ex.LineNo, ex.ColumnNo, ex.Message, context.GetText());
                e2.InnerMessage = ex.InnerException?.Message ?? "";
                compileErrors.Add(e2);


            } catch (Exception ex) {

                var e1 = new CompilationError(context.Start.Line, context.Start.Column, ex.Message, context.GetText());
                e1.InnerMessage = ex.InnerException?.Message ?? "";
                compileErrors.Add(e1);

            } finally {
                
            }
        }

        return $"var {variable};";
    }

}