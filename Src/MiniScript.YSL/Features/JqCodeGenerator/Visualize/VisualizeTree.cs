using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Text;

namespace TunnelSoft.YSL.Features.CodeGenerator;

public partial class JQueryCodeGeneratorVisitor {

    // Override VisitProgram to begin code generation
    public string VisualizeTree(int indent) {
        StringBuilder visualizedTree = new StringBuilder();
        VisualizeTreeHelper(_tree, indent, visualizedTree);
        return visualizedTree.ToString();
    }

    static void VisualizeTreeHelper(IParseTree tree, int indent, StringBuilder visualizedTree) {
        for (int i = 0; i < indent; i++) {
            visualizedTree.Append("  ");
        }
        visualizedTree.Append("├── ");
        visualizedTree.AppendLine(GetNodeText(tree));

        for (int i = 0; i < tree.ChildCount; i++) {
            if (i == tree.ChildCount - 1) {
                for (int j = 0; j < indent; j++) {
                    visualizedTree.Append("  ");
                }
                visualizedTree.Append("└── ");
                VisualizeTreeHelper(tree.GetChild(i), indent + 1, visualizedTree);
            } else {
                VisualizeTreeHelper(tree.GetChild(i), indent + 1, visualizedTree);
            }
        }
    }

    static string GetNodeText(IParseTree tree) {
        if (tree is TerminalNodeImpl) {
            var token = (CommonToken)((TerminalNodeImpl)tree).Symbol;
            switch (token.Type) {
                case MiniScriptLexer.IDENTIFIER:
                    return "Identifier: " + token.Text;
                case MiniScriptLexer.NUMBER:
                    return "Number: " + token.Text;
                case MiniScriptLexer.STRING:
                    return "String: " + token.Text;
                default:
                    return token.Text;
            }
        } else {
            return tree.ToString();
        }
    }

}