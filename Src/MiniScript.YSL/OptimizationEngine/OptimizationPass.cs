using System.Text;
using System.Text.RegularExpressions;
using TunnelSoft.MiniScript.YSL.Symbols;

namespace TunnelSoft.MiniScript.YSL.OptimizationEngine;

public class OptimizationPass {
    public bool RemoveUnusedVariables { get; set; } = true;
    public bool CombineConsecutiveAssignments { get; set; } = true;
    public bool SimplifyExpressions { get; set; } = true;
    public bool InlineSimpleFunctions { get; set; } = true;
    public bool OptimizeLoops { get; set; } = true;

    private readonly EnhancedSymbolTable symbolTable;

    public OptimizationPass(EnhancedSymbolTable symbolTable) {
        this.symbolTable = symbolTable;
    }

    public string Optimize(string code) {
        var optimizedCode = code;

        if (RemoveUnusedVariables) {
            optimizedCode = RemoveUnusedVariablesFromCode(optimizedCode);
        }

        if (CombineConsecutiveAssignments) {
            optimizedCode = CombineConsecutiveAssignmentsFromCode(optimizedCode);
        }

        if (SimplifyExpressions) {
            optimizedCode = SimplifyExpressionsFromCode(optimizedCode);
        }

        if (InlineSimpleFunctions) {
            optimizedCode = InlineSimpleFunctionsFromCode(optimizedCode);
        }

        if (OptimizeLoops) {
            optimizedCode = OptimizeLoopsFromCode(optimizedCode);
        }

        return optimizedCode;
    }

    private string RemoveUnusedVariablesFromCode(string code) {
        var unusedSymbols = symbolTable.GetUnusedSymbols();
        foreach (var symbol in unusedSymbols) {
            var pattern = $@"\bvar\s+{symbol}\s*=\s*[^;]+;";
            code = Regex.Replace(code, pattern, "");
        }
        return code;
    }

    private string CombineConsecutiveAssignmentsFromCode(string code) {
        var pattern = @"(\w+)\s*=\s*([^;]+);\s*\1\s*=\s*([^;]+);";
        return Regex.Replace(code, pattern, "$1 = $3;");
    }

    private string SimplifyExpressionsFromCode(string code) {
        // Implement constant folding
        code = Regex.Replace(code, @"(\d+)\s*\+\s*(\d+)", m =>
            (int.Parse(m.Groups[1].Value) + int.Parse(m.Groups[2].Value)).ToString());

        // Simplify boolean expressions
        code = Regex.Replace(code, @"true\s*&&\s*(\w+)", "$1");
        code = Regex.Replace(code, @"false\s*\|\|\s*(\w+)", "$1");

        return code;
    }

    private string InlineSimpleFunctionsFromCode(string code) {
        // This is a simplified version. In practice, you'd need to analyze function usage and complexity.
        var functionPattern = @"function\s+(\w+)\s*\(\)\s*{\s*return\s+([^;]+);\s*}";
        var matches = Regex.Matches(code, functionPattern);

        foreach (Match match in matches) {
            var functionName = match.Groups[1].Value;
            var returnValue = match.Groups[2].Value;
            code = Regex.Replace(code, $@"\b{functionName}\(\)", returnValue);
        }

        return code;
    }

    private string OptimizeLoopsFromCode(string code) {
        // Unroll small loops
        var loopPattern = @"for\s*\(var\s+(\w+)\s*=\s*(\d+);\s*\1\s*<\s*(\d+);\s*\1\+\+\)\s*{([^}]+)}";
        code = Regex.Replace(code, loopPattern, m => {
            var variable = m.Groups[1].Value;
            var start = int.Parse(m.Groups[2].Value);
            var end = int.Parse(m.Groups[3].Value);
            var body = m.Groups[4].Value;

            if (end - start <= 3) { // Unroll only small loops
                var unrolled = new StringBuilder();
                for (int i = start; i < end; i++) {
                    unrolled.Append(body.Replace(variable, i.ToString()));
                }
                return unrolled.ToString();
            }
            return m.Value; // Return original if not unrolling
        });

        return code;
    }
}
