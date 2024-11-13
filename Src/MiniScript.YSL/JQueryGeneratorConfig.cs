namespace TunnelSoft.MiniScript.YSL;

public class JQueryGeneratorConfig {
    public bool UseShorthand { get; set; } // $() vs jQuery()
    public bool MinifyOutput { get; set; }
    public string SelectorPrefix { get; set; }
}
