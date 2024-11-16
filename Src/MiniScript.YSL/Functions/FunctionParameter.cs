using System.ComponentModel.DataAnnotations;

namespace TunnelSoft.MiniScript.YSL.Functions;

public class FunctionParameter {
    public string Name { get; set; }
    public DataType Type { get; set; }
    public object DefaultValue { get; set; }
    public bool IsOptional => DefaultValue != null;
}
