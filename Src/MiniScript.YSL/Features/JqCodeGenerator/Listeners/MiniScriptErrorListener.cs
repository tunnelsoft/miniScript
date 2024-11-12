using Antlr4.Runtime;

namespace TunnelSoft.YSL.Features.CodeGenerator;
public class MiniScriptErrorListener : IAntlrErrorListener<IToken> {

    private readonly List<CompilationError> errors = new List<CompilationError>();

    public List<CompilationError> Errors => errors;

    public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
        var error = new CompilationError(line, charPositionInLine, msg, "", offendingSymbol.Text);
        errors.Add(error);
    }
}
