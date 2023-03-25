using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    public BTSelector(string name) : base(name)
    {
        nodeName = name;
    }

    public override NodeState Process()
    {
        NodeState childState = children[currentChild].Process();
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;
        if (childState == NodeState.SUCCESS)
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }

        currentChild++;
        if(currentChild>= children.Count)
        {
            currentChild = 0;
            return NodeState.FAILURE;
        }
        


        return NodeState.RUNNING;
    }

}
