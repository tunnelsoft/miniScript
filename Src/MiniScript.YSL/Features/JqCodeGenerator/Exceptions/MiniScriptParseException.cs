namespace TunnelSoft.YSL.Features.CodeGenerator;

[Serializable]
public class MiniScriptParseException : Exception {


    public int LineNo { get; set; }
    public int ColumnNo { get; set; }
    public string Expression { get; set; }


    public MiniScriptParseException() {
    }

    public MiniScriptParseException(string? message, int lineNo, int columnNo, string expression) : base(message) {
        LineNo = lineNo;
        ColumnNo = columnNo;
        Expression = expression;
    }

    public MiniScriptParseException(string? message, Exception? innerException) : base(message, innerException) {
    }
}