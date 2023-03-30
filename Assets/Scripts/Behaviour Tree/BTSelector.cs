using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    public BTSelector(string name) : base(name)
    {
        nodeName = name;
    }

    // Overriding the Process method defined in the base class BTNode
    public override NodeState Process()
    {
        // Calling the Process method of the current child node and storing the returned state in a variable called childState
        NodeState childState = children[currentChild].Process();

        // If the child node is still running, return the state RUNNING
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;

        // If the child node has succeeded, reset the currentChild variable to 0 and return the state SUCCESS
        if (childState == NodeState.SUCCESS)
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }

        // If the child node has failed, increment the currentChild variable by 1
        currentChild++;

        // If the currentChild variable is greater than or equal to the number of children, reset it to 0 and return the state FAILURE
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeState.FAILURE;
        }

        // If none of the above conditions are met, return the state RUNNING
        return NodeState.RUNNING;
    }

}
