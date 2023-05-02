using UnityEngine.AI;

public class BTDependencySequence : BTNode
{
    readonly BehaviourTree DependencyTree;
    readonly NavMeshAgent agent;
    public BTDependencySequence(string name, BehaviourTree tree, NavMeshAgent navAgent)

    {
        nodeName = name;
        DependencyTree = tree;
        agent = navAgent;
    }

    // An overridden method of the BTNode class which returns the state of the current node
    public override NodeState Process()
    {
        // If the dependancy tree fails, reset the path and return failure
        if (DependencyTree.Process() == NodeState.FAILURE)
        {
            agent.ResetPath();
            foreach (BTNode child in children)
            {
                child.Reset();
            }
            return NodeState.FAILURE;
        }


        // Get the state of the current child node
        NodeState childState = children[currentChild].Process();

        // If the child node is still running, return running state
        if (childState == NodeState.RUNNING) return NodeState.RUNNING;

        // If the child node fails, return failure state
        if (childState == NodeState.FAILURE) return childState;

        // If the child node succeeds, move on to the next child node
        currentChild++;

        // If all child nodes have been processed, reset the currentChild index and return success state
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }

        // If there are still child nodes left to be processed, return running state
        return NodeState.RUNNING;
    }
}
