using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelSoft.MiniScript.YSL.ControlFlowGraph;

public class ControlFlowGraph {
    private readonly Dictionary<int, BasicBlock> blocks;
    private BasicBlock entryBlock;
    private BasicBlock exitBlock;
    private int nextBlockId;

    public ControlFlowGraph() {
        blocks = new Dictionary<int, BasicBlock>();
        nextBlockId = 0;
    }

    public BasicBlock CreateBlock() {
        var block = new BasicBlock(nextBlockId++);
        blocks[block.Id] = block;
        return block;
    }

    public void AddEdge(BasicBlock from, BasicBlock to) {
        if (!from.Successors.Contains(to)) {
            from.Successors.Add(to);
            to.Predecessors.Add(from);
        }
    }

    public void BuildFromAST(ASTNode root) {
        entryBlock = CreateBlock();
        exitBlock = CreateBlock();
        entryBlock.IsEntry = true;
        exitBlock.IsExit = true;

        var visitor = new CFGBuilder(this);
        visitor.Visit(root);
    }

    public IEnumerable<BasicBlock> GetReachableBlocks() {
        var visited = new HashSet<BasicBlock>();
        var queue = new Queue<BasicBlock>();
        queue.Enqueue(entryBlock);

        while (queue.Count > 0) {
            var block = queue.Dequeue();
            if (visited.Add(block)) {
                foreach (var successor in block.Successors) {
                    queue.Enqueue(successor);
                }
            }
        }

        return visited;
    }

    public bool HasPath(BasicBlock from, BasicBlock to) {
        var visited = new HashSet<BasicBlock>();
        return HasPathDFS(from, to, visited);
    }

    private bool HasPathDFS(BasicBlock current, BasicBlock target, HashSet<BasicBlock> visited) {
        if (current == target)
            return true;
        if (!visited.Add(current))
            return false;

        return current.Successors.Any(successor => HasPathDFS(successor, target, visited));
    }

    public void Optimize() {
        RemoveUnreachableBlocks();
        MergeBlocks();
    }

    private void RemoveUnreachableBlocks() {
        var reachable = GetReachableBlocks().ToHashSet();
        var unreachable = blocks.Values.Except(reachable).ToList();

        foreach (var block in unreachable) {
            foreach (var pred in block.Predecessors.ToList()) {
                pred.Successors.Remove(block);
            }
            foreach (var succ in block.Successors.ToList()) {
                succ.Predecessors.Remove(block);
            }
            blocks.Remove(block.Id);
        }
    }

    private void MergeBlocks() {
        bool changed;
        do {
            changed = false;
            foreach (var block in blocks.Values.ToList()) {
                if (block.Successors.Count == 1) {
                    var successor = block.Successors[0];
                    if (successor.Predecessors.Count == 1) {
                        MergeBlocks(block, successor);
                        changed = true;
                    }
                }
            }
        } while (changed);
    }

    private void MergeBlocks(BasicBlock first, BasicBlock second) {
        first.Instructions.AddRange(second.Instructions);
        first.Successors.Clear();
        first.Successors.AddRange(second.Successors);

        foreach (var successor in second.Successors) {
            var index = successor.Predecessors.IndexOf(second);
            successor.Predecessors[index] = first;
        }

        blocks.Remove(second.Id);
    }
}
