namespace TunnelSoft.MiniScript.YSL.Symbols;
using System.Collections.Generic;

// CompilationError class to represent errors
public class CompilationError {
    public int Line { get; }
    public int Column { get; }
    public string Message { get; }
    public string OffendingSymbol { get; }
    public string Expression { get; }

    public string InnerMessage { get; set; }

    public CompilationError(int line, int column, string message, string expression, string? offendingSymbol = null) {
        Line = line;
        Column = column;
        Message = message;
        Expression = expression;
        OffendingSymbol = offendingSymbol ?? "";
    }
}

