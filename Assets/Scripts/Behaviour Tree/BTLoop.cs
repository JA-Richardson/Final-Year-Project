using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLoop : BTNode
{
    BehaviourTree dependancy;
    public BTLoop(string name, BehaviourTree tree)
    {
        nodeName = name;
        dependancy = tree;
    }

    public override NodeState Process()
    {

        if (dependancy.Process() == NodeState.FAILURE)
        {
            return NodeState.SUCCESS;
        }

        NodeState childState = children[currentChild].Process();

        if (childState == NodeState.RUNNING) return NodeState.RUNNING;
        if (childState == NodeState.FAILURE)
        {
            currentChild = 0;
            foreach (BTNode child in children)
            {
                child.Reset();
            }
            return childState;
        }

        if (childState == NodeState.FAILURE) return childState;

        currentChild++;

        if (currentChild >= children.Count)
        {
            currentChild = 0;
        }

        return NodeState.RUNNING;
    }
}
