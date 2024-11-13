namespace TunnelSoft.MiniScript.YSL.SourceMap;
public class SourceMapEntry {
    public int OriginalLine { get; set; }
    public int OriginalColumn { get; set; }
    public int GeneratedLine { get; set; }
    public int GeneratedColumn { get; set; }
    public string SourceFile { get; set; }
    public string Name { get; set; }
}
