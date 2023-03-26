using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInverter : BTNode
{
    public BTInverter(string name)
    {
        nodeName = name;
    }

    public override NodeState Process()
    {
        NodeState childState = children[currentChild].Process();
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;
        if (childState == NodeState.FAILURE) return NodeState.SUCCESS;
        else 
            return NodeState.FAILURE;
        
    }
}
