using Microsoft.AspNetCore.Http;

namespace TunnelSoft.YSL.Features.CodeGenerator;
public class Compile_Request {
    public Compile_Request() {
            
    }

    public string? CodeText { get; set; }

    public IFormFile CodeFile { get; set; }

}
