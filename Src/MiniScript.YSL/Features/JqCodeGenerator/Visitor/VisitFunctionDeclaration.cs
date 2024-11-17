using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelSoft.MiniScript.YSL.Functions;

namespace TunnelSoft.YSL.Features.CodeGenerator;

public partial class JQueryCodeGeneratorVisitor {
    public override string VisitFunctionDeclaration(MiniScriptParser.FunctionDeclarationContext context) {
        var functionName = context.IDENTIFIER().GetText();
        var parameters = VisitParameterList(context.parameterList());

        var functionDecl = new FunctionDeclaration {
            Name = functionName,
            //Parameters = parameters,
            Body = context.block(),
            DeclarationLine = context.Start.Line,
            SourceFile =  ""  // currentSourceFile // Add this field to track source file
        };

        // Create new CFG for function
        //var functionCfg = new ControlFlowGraph();
        //cfgStack.Push(functionCfg);

        //try {
        //    // Process function body
        //    var bodyCode = Visit(context.block());

        //    // Build CFG from function body
        //    functionCfg.BuildFromAST(context.block());

        //    // Optimize the CFG
        //    functionCfg.Optimize();

        //    // Register function
        //    functionManager.RegisterFunction(functionDecl);

        //    // Generate function code
        //    return GenerateFunctionCode(functionName, parameters, bodyCode);
        //} finally {
        //    cfgStack.Pop();
        //}
    }

    private string GenerateFunctionCode(string name, List<FunctionParameter> parameters, string body) {
        var paramList = string.Join(", ", parameters.Select(p => {
            if (p.IsOptional)
                //return $"{p.Name} = {Visit(p.DefaultValue)}";
            return p.Name;
        }));

        return $"function {name}({paramList}) {{\n{body}\n}}";
    }
}
