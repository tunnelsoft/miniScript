namespace TunnelSoft.MiniScript.YSL.Functions;

public class FunctionManager {
    private readonly Dictionary<string, FunctionDeclaration> functions = new Dictionary<string, FunctionDeclaration>();
    private readonly Stack<FunctionDeclaration> callStack = new Stack<FunctionDeclaration>();

    public void RegisterFunction(FunctionDeclaration function) {
        if (functions.ContainsKey(function.Name)) {
            throw new InvalidOperationException($"Function '{function.Name}' already declared");
        }
        functions[function.Name] = function;
    }

    public FunctionDeclaration GetFunction(string name) {
        return functions.TryGetValue(name, out var function) ? function : null;
    }

    public void EnterFunction(FunctionDeclaration function) {
        callStack.Push(function);
    }

    public void ExitFunction() {
        if (callStack.Count > 0) {
            callStack.Pop();
        }
    }

    public FunctionDeclaration CurrentFunction => callStack.Count > 0 ? callStack.Peek() : null;

    public bool IsRecursive(string functionName) {
        return callStack.Any(f => f.Name == functionName);
    }
}
