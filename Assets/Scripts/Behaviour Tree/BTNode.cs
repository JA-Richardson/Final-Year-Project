using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode
{
    public enum NodeState{ SUCCESS, FAILURE, RUNNING };
    public NodeState nodeState;
    public List<BTNode> children = new();
    public int currentChild = 0;
    public string nodeName;
    
    public BTNode() { }
    
    public BTNode(string name)
    {
        nodeName = name;
    }

    public virtual NodeState Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(BTNode child)
    {
        children.Add(child);
    }
}
