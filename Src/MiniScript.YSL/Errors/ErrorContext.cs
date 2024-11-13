
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelSoft.MiniScript.YSL.Errors;
public class ErrorContext {
    public ErrorSeverity Severity { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
    public bool IsFatal { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }
    public string SourceFragment { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    public List<string> SuggestedFixes { get; set; }

    public ErrorContext() {
        Metadata = new Dictionary<string, object>();
        SuggestedFixes = new List<string>();
    }
}
