using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : BTNode
{
    public BehaviourTree()
    {
        nodeName = "Behaviour Tree";
    }

    public BehaviourTree(string name)
    {
        nodeName = name;
    }

    public override NodeState Process()
    {
        if (children.Count == 0) return NodeState.SUCCESS;
        return children[currentChild].Process();
    }

    struct BTNodeLevel
    {
        public BTNode node;
        public int level;
    }

    public void PrintTree()
    {
        string treePrintout = "";
        Stack<BTNodeLevel> stack = new Stack<BTNodeLevel>();
        BTNode currentNode = this;
        stack.Push(new BTNodeLevel { level = 0, node = currentNode });
        
        while (stack.Count != 0)
        {
            BTNodeLevel nextNode = stack.Pop();
            treePrintout += new string ('-', nextNode.level) +nextNode.node.nodeName + "\n";
            for (int i = nextNode.node.children.Count - 1; i >= 0; i--)
            {
                stack.Push(new BTNodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i] });
            }
        }
        Debug.Log(treePrintout);
    }
}
