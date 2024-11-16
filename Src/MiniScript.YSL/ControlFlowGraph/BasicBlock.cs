namespace TunnelSoft.MiniScript.YSL.ControlFlowGraph;

public class BasicBlock {
    public int Id { get; set; }
    public List<ASTNode> Instructions { get; }
    public List<BasicBlock> Successors { get; }
    public List<BasicBlock> Predecessors { get; }
    public bool IsEntry { get; set; }
    public bool IsExit { get; set; }

    public BasicBlock(int id) {
        Id = id;
        Instructions = new List<ASTNode>();
        Successors = new List<BasicBlock>();
        Predecessors = new List<BasicBlock>();
    }
}
