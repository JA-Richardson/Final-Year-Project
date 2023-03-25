using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTNode
{
    public BTSequence(string name)
    {
        nodeName = name;
    }

    public override NodeState Process()
    {
        NodeState childState = children[currentChild].Process();
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;
        if (childState == NodeState.FAILURE) return childState;
        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }


        return NodeState.RUNNING;
    }
}
