using TunnelSoft.MiniScript.YSL.Symbols;
using TunnelSoft.Utilities;

namespace TunnelSoft.YSL.Features.CodeGenerator;

public partial class JQueryCodeGeneratorVisitor {

    // Override VisitProgram to begin code generation
    public override string VisitProgram(MiniScriptParser.ProgramContext context) {
        // Wrap the code in $(document).ready()
        //TODO: remove this part
        //jqueryBuilder.AppendLine("$(document).ready(function() {");

        symbolTable.EnterScope(); // Enter global scope
        try {


            foreach (var statement in context.statement()) {
                var stmtCode = Visit(statement);
                if (!string.IsNullOrWhiteSpace(stmtCode))
                    jqueryBuilder.AppendLine("    " + stmtCode);
            }
        } catch (MiniScriptParseException ex) {

            var e2 = new CompilationError(context.Start.Line, context.Start.Column, ex.Message, context.GetText());
            e2.InnerMessage = ex.InnerException?.Message ?? "";
            compileErrors.Add(e2);

        } catch (Exception ex) {

            var e1 = new CompilationError(context.Start.Line, context.Start.Column, ex.Message, context.GetText());
            e1.InnerMessage = ex.InnerException?.Message ?? "";
            compileErrors.Add(e1);

        } finally {
            symbolTable.ExitScope(); // Exit global scope
        }


        //TODO: remove this part 
        //jqueryBuilder.AppendLine("});"); // Close document ready

        return jqueryBuilder.ToString().TrimEnd(Environment.NewLine); // The code is built incrementally
    }

}