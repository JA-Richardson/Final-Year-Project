public class BTSelector : BTNode
{
    public BTSelector(string n)
    {
        nodeName = n;
    }

    public override NodeState Process()
    {
        // Get the state of the current child node
        NodeState childstatus = children[currentChild].Process();
        if (childstatus == NodeState.RUNNING) return NodeState.RUNNING;
        // If the child node succeeds, move on to the next child node
        if (childstatus == NodeState.SUCCESS)
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }
        // If all child nodes have been processed, reset the currentChild index and return failure state
        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeState.FAILURE;
        }

        return NodeState.RUNNING;
    }

}
