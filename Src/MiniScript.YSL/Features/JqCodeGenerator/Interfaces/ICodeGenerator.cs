using TunnelSoft.Utilities.Result;

namespace TunnelSoft.YSL.Features.CodeGenerator.Interfaces;
public interface ICodeGenerator {
    Task<ServiceResult<Compile_Response>> GenerateCode(string input, string? fileName = null);
    string GetStatistics();
}
