using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    public BTSelector(string n)
    {
        nodeName = n;
    }

    public override NodeState Process()
    {
        NodeState childstatus = children[currentChild].Process();
        if (childstatus == NodeState.RUNNING) return NodeState.RUNNING;

        if (childstatus == NodeState.SUCCESS)
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeState.FAILURE;
        }

        return NodeState.RUNNING;
    }

}
