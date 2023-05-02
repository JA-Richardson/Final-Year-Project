public class BTLoop : BTNode
{
    readonly BehaviourTree dependancy;
    public BTLoop(string name, BehaviourTree tree)
    {
        nodeName = name;
        dependancy = tree;
    }

    public override NodeState Process()
    {
        // If the dependancy tree fails, return success
        if (dependancy.Process() == NodeState.FAILURE)
        {
            return NodeState.SUCCESS;
        }
        
        NodeState childState = children[currentChild].Process();
        // If the child node is still running, return running state
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;
        if (childState == NodeState.FAILURE)
        {
            // Reset the dependancy tree if the child node fails
            currentChild = 0;
            foreach (BTNode child in children)
            {
                child.Reset();
            }
            return childState;
        }
        // If the child node fails, return failure state
        if (childState == NodeState.FAILURE) return childState;

        currentChild++;
        // If all child nodes have been processed, reset the currentChild index and return success state
        if (currentChild >= children.Count)
        {
            currentChild = 0;
        }

        return NodeState.RUNNING;
    }
}
