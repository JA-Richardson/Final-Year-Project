public class BTInverter : BTNode
{
    public BTInverter(string name)
    {
        nodeName = name;
    }

    // Override the Process() method inherited from BTNode
    public override NodeState Process()
    {
        // Get the current state of the child node
        NodeState childState = children[0].Process();

        // If the child node is running, return running
        if (childState == NodeState.RUNNING)
            return NodeState.RUNNING;

        // If the child node failed, return success
        if (childState == NodeState.FAILURE)
            return NodeState.SUCCESS;

        // If the child node succeeded, return failure
        else
            return NodeState.FAILURE;
    }
}
