using System.Text;
using TunnelSoft.MiniScript.YSL.Symbols;
using static TunnelSoft.YSL.Features.CodeGenerator.Constants;


namespace TunnelSoft.YSL.Features.CodeGenerator;
public partial class JQueryCodeGeneratorVisitor {


    // List to collect errors
    private readonly List<CompilationError> compileErrors = new List<CompilationError>();

    // Method to retrieve collected errors
    public string GetErrors() {

        StringBuilder errorBuilder = new StringBuilder();

        if (errorListener.Errors.Count > 0) {
            errorBuilder.AppendLine("Compilation Errors:");
            foreach (var error in errorListener.Errors) {
                errorBuilder.AppendLine($"Error at line {error.Line}, column {error.Column}, Symbol `{error.OffendingSymbol}`, Message: {error.Message}");
            }
        }


        if (compileErrors.Count == 0 && errorBuilder.Length == 0)
            return NO_ERROR_FOUND;

        foreach (var error in compileErrors) {
            errorBuilder.AppendLine($"{error.Line}:{error.Column} - {error.Message}");
        }
        return errorBuilder.ToString();
    }

}
