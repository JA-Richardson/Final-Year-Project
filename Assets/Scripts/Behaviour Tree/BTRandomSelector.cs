public class BTRandomSelector : BTNode
{
    // Keep track of whether the children have been shuffled
    bool shuffled = false;

    public BTRandomSelector(string name) : base(name)
    {
        nodeName = name;
    }

    public override NodeState Process()
    {
        // Check if children have been shuffled yet
        if (!shuffled)
        {
            children.Shuffle();
            shuffled = true;
        }

        // Execute the child node at the currentChild index and store its NodeState
        NodeState childState = children[currentChild].Process();

        // If the child is still running, return RUNNING to indicate the behavior tree is still executing
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;

        // If the child has succeeded, reset the currentChild index to zero, set shuffled to false, and return SUCCESS
        if (childState == NodeState.SUCCESS)
        {
            currentChild = 0;
            shuffled = false;
            return NodeState.SUCCESS;
        }

        // If the child has failed, increment the currentChild index and check if it exceeds the number of children.
        // If it does, reset the currentChild index to zero, set shuffled to false, and return FAILURE.
        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            shuffled = false;
            return NodeState.FAILURE;
        }

        // If none of the above conditions are met, increment the currentChild index and return RUNNING.
        return NodeState.RUNNING;
    }
}

