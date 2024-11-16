namespace TunnelSoft.MiniScript.YSL.SL;
public static class StandardLibrary {
    private static readonly Dictionary<string, Func<object[], object>> Functions;
    private static readonly Dictionary<string, object> Constants;

    static StandardLibrary() {
        Functions = new Dictionary<string, Func<object[], object>> {
            // Console operations
            { "print", args => { Console.WriteLine(string.Join(" ", args)); return null; } },
            { "input", args => Console.ReadLine() },

            // Type conversion
            { "toString", args => args[0]?.ToString() },
            { "toNumber", args => Convert.ToDouble(args[0]) },
            { "toBoolean", args => Convert.ToBoolean(args[0]) },

            // Array operations
            { "length", args => ((Array)args[0]).Length },
            { "push", args => {
                var list = ((List<object>)args[0]);
                list.Add(args[1]);
                return list.Count;
            }},
            { "pop", args => {
                var list = ((List<object>)args[0]);
                var item = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return item;
            }},

            // Math operations
            { "abs", args => Math.Abs(Convert.ToDouble(args[0])) },
            { "max", args => args.Select(a => Convert.ToDouble(a)).Max() },
            { "min", args => args.Select(a => Convert.ToDouble(a)).Min() },
            { "round", args => Math.Round(Convert.ToDouble(args[0])) },
            { "random", args => new Random().NextDouble() },

            // String operations
            { "substring", args => ((string)args[0]).Substring(
                Convert.ToInt32(args[1]),
                Convert.ToInt32(args[2])) },
            { "replace", args => ((string)args[0]).Replace((string)args[1], (string)args[2]) },
            { "split", args => ((string)args[0]).Split((string)args[1]).ToList() },

            // Date/Time operations
            { "now", args => DateTime.Now },
            { "timestamp", args => DateTimeOffset.Now.ToUnixTimeMilliseconds() },
        };

        Constants = new Dictionary<string, object> {
            { "PI", Math.PI },
            { "E", Math.E },
            { "INFINITY", double.PositiveInfinity },
            { "NAN", double.NaN },
            { "VERSION", "1.0.0" }
        };
    }



    public static IEnumerable<string> GetFunctionNames() {
        return Functions.Keys;
    }


    public static object InvokeFunction(string name, object[] arguments) {
        if (!Functions.TryGetValue(name, out var function)) {
            throw new InvalidOperationException($"Function '{name}' not found in standard library");
        }

        try {
            return function(arguments);
        } catch (Exception ex) {
            throw new InvalidOperationException($"Error executing function '{name}': {ex.Message}", ex);
        }
    }

    public static bool HasFunction(string name) {
        return Functions.ContainsKey(name);
    }

    public static object GetConstant(string name) {
        if (!Constants.TryGetValue(name, out var value)) {
            throw new InvalidOperationException($"Constant '{name}' not found in standard library");
        }
        return value;
    }

    public static bool HasConstant(string name) {
        return Constants.ContainsKey(name);
    }

    public static class Collections {
        public static object Map(IEnumerable<object> collection, Func<object, object> callback) {
            return collection.Select(callback).ToList();
        }

        public static object Filter(IEnumerable<object> collection, Func<object, bool> predicate) {
            return collection.Where(predicate).ToList();
        }

        public static object Reduce(IEnumerable<object> collection, Func<object, object, object> callback, object initial) {
            return collection.Aggregate(initial, callback);
        }
    }

    public static class Debug {
        public static void Assert(bool condition, string message = null) {
            if (!condition) {
                throw new AssertionException(message ?? "Assertion failed");
            }
        }

        public static void Log(object value, string level = "info") {
            // Implement logging logic
        }
    }
}

public class AssertionException : Exception {
    public AssertionException(string message) : base(message) { }
}