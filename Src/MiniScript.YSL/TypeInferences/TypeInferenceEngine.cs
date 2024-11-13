using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelSoft.MiniScript.YSL.Symbols.Base;

namespace TunnelSoft.MiniScript.YSL.TypeInferences;

public partial class TypeInferenceEngine {

    private readonly Dictionary<string, TypeInfo> typeCache = new Dictionary<string, TypeInfo>();

    public TypeInfo InferType(MiniScriptParser.ExpressionContext context) {
        var typeInfo = new TypeInfo();

        if (context.comparisonExpression() != null) {
            typeInfo.BaseType = SymbolType.Variable;
            typeInfo.PossibleTypes.Add("boolean");
            return typeInfo;
        }

        // Add more type inference logic based on context
        return InferFromContext(context);
    }

    private TypeInfo InferFromContext(ParserRuleContext context) {
        // Implementation of context-based type inference
        var typeInfo = new TypeInfo();

        // Add inference logic based on context type
        if (context is MiniScriptParser.UnaryExpressionContext) {
            typeInfo.BaseType = SymbolType.Variable;
            typeInfo.PossibleTypes.Add("number");
        }
        // Add more context checks

        return typeInfo;
    }

    public void UpdateTypeInfo(string symbol, TypeInfo newType) {
        if (typeCache.ContainsKey(symbol)) {
            // Merge existing type information with new information
            MergeTypeInfo(typeCache[symbol], newType);
        } else {
            typeCache[symbol] = newType;
        }
    }

    private void MergeTypeInfo(TypeInfo existing, TypeInfo newInfo) {
        existing.PossibleTypes.UnionWith(newInfo.PossibleTypes);
        // Add more merging logic as needed
    }
}
