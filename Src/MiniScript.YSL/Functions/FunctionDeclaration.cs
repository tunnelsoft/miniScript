using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelSoft.MiniScript.YSL.Symbols.Base;

namespace TunnelSoft.MiniScript.YSL.Functions;

public class FunctionDeclaration {
    public string Name { get; set; }
    public List<FunctionParameter> Parameters { get; }
    public DataType ReturnType { get; set; }
    public MiniScriptParser.BlockContext Body { get; set; }
    public SymbolTable ClosureScope { get; }
    public bool IsAsync { get; set; }
    public HashSet<string> CapturedVariables { get; }
    public string SourceFile { get; set; }
    public int DeclarationLine { get; set; }
    public bool IsBuiltin { get; set; } = false;

    public FunctionDeclaration() {
        Parameters = new List<FunctionParameter>();
        ClosureScope = new SymbolTable();
        CapturedVariables = new HashSet<string>();
    }

    public object Invoke(object[] arguments) {
        // Create new scope for function execution
        ClosureScope.EnterScope();

        try {
            // Bind parameters to arguments
            BindParameters(arguments);

            // Execute function body
            //var visitor = new FunctionExecutionVisitor(ClosureScope);
            //var result = visitor.Visit(Body);

            return "";
        } finally {
            ClosureScope.ExitScope();
        }
    }

    private void BindParameters(object[] arguments) {
        for (int i = 0; i < Parameters.Count; i++) {
            var parameter = Parameters[i];
            var value = i < arguments.Length ? arguments[i] : parameter.DefaultValue;

            if (value == null && !parameter.IsOptional) {
                throw new ArgumentException($"Required parameter '{parameter.Name}' not provided");
            }

            ClosureScope.Declare(parameter.Name, SymbolType.Variable);
            // Store the value in the current scope
        }
    }
}
