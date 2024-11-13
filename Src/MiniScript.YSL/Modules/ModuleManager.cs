namespace TunnelSoft.MiniScript.YSL.Modules;

public class ModuleManager {
    private readonly Dictionary<string, ModuleContext> modules =
        new Dictionary<string, ModuleContext>();
    private readonly Stack<ModuleContext> moduleStack = new Stack<ModuleContext>();

    public ModuleContext CreateModule(string name) {
        var module = new ModuleContext(name);
        modules[name] = module;
        return module;
    }

    public void EnterModule(string name) {
        if (modules.TryGetValue(name, out var module)) {
            moduleStack.Push(module);
        }
    }

    public void ExitModule() {
        if (moduleStack.Count > 0) {
            moduleStack.Pop();
        }
    }

    public ModuleContext CurrentModule => moduleStack.Peek();

    public bool ValidateModuleDependencies() {
        foreach (var module in modules.Values) {
            foreach (var import in module.Imports) {
                if (!modules.ContainsKey(import.Value)) {
                    return false;
                }
            }
        }
        return true;
    }
}
