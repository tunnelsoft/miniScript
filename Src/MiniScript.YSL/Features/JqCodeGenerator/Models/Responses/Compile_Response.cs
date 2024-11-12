namespace TunnelSoft.YSL.Features.CodeGenerator;
public class Compile_Response {
    public Compile_Response() {
        
    }
    public bool IsCompileSuccess { get; set; }
    public string? CompiledCode { get; set; }
    public string? TreeText { get; set; }
    public string? VisualTree { get; set; }
    public string? Statistics { get; set; }
    public string? CompileErrors { get; set; }
}
