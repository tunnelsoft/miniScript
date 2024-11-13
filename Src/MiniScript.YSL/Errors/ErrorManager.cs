using System.Text;

namespace TunnelSoft.MiniScript.YSL.Errors;
public class ErrorManager {
    private readonly List<ErrorContext> errors = new List<ErrorContext>();
    private readonly Dictionary<string, int> errorCounts = new Dictionary<string, int>();
    private readonly int maxErrorsPerType;

    public ErrorManager(int maxErrorsPerType = 10) {
        this.maxErrorsPerType = maxErrorsPerType;
    }

    public void AddError(ErrorContext error) {
        if (!errorCounts.ContainsKey(error.Code)) {
            errorCounts[error.Code] = 0;
        }

        if (errorCounts[error.Code]++ < maxErrorsPerType) {
            errors.Add(error);
        }
    }

    public string GenerateReport() {
        var report = new StringBuilder();
        var groupedErrors = errors.GroupBy(e => e.Severity)
                                .OrderByDescending(g => g.Key);

        foreach (var group in groupedErrors) {
            report.AppendLine($"{group.Key} ({group.Count()}):");
            foreach (var error in group.OrderBy(e => e.Line)) {
                report.AppendLine($"  [{error.Code}] Line {error.Line}:{error.Column} - {error.Message}");
                if (error.SuggestedFixes.Any()) {
                    report.AppendLine("    Suggested fixes:");
                    foreach (var fix in error.SuggestedFixes) {
                        report.AppendLine($"    - {fix}");
                    }
                }
            }
        }

        return report.ToString();
    }
}
