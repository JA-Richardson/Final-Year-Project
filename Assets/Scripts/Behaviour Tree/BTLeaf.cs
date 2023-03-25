using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf : BTNode
{
    public delegate NodeState Tick();
    public Tick StateMethod;

    public BTLeaf() { }
    
    public BTLeaf(string name, Tick sm)
    {
        nodeName = name;
        StateMethod = sm;
    }

    public override NodeState Process()
    {
        if (StateMethod != null)
            return StateMethod();
        return NodeState.FAILURE;
    }
}
