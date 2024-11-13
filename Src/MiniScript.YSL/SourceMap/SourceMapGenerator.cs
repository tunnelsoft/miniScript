using Antlr4.Runtime;

namespace TunnelSoft.MiniScript.YSL.SourceMap;

public class SourceMapGenerator {
    private readonly SourceMap sourceMap = new SourceMap();
    private int currentGeneratedLine = 1;
    private int currentGeneratedColumn = 0;

    public void AddMapping(IToken token, string sourceFile) {
        sourceMap.AddMapping(
            token.Line,
            token.Column,
            currentGeneratedLine,
            currentGeneratedColumn,
            sourceFile,
            token.Text
        );
    }

    public void NewLine() {
        currentGeneratedLine++;
        currentGeneratedColumn = 0;
    }

    public void AddColumn(int columns) {
        currentGeneratedColumn += columns;
    }

    public string GenerateSourceMap() {
        return sourceMap.GenerateSourceMap();
    }

    public void TrackSymbol(string symbol, IToken token) {
        sourceMap.TrackSymbol(symbol, token.Line);
    }
}
