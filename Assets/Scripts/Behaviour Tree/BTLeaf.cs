using System.Diagnostics;

public class BTLeaf : BTNode
{
    // Define a delegate for the state of the node
    // This delegate will be called to determine the state of the node
    public delegate NodeState Tick();
    // Define the method for the state of the node
    // This method will be called to determine the state of the node
    public Tick StateMethod;

    // Define a delegate for multi-state node
    // This delegate will be called to determine the state of the node based on the specified index
    public delegate NodeState MultiTick(int index);
    // Define the method for the multi-state node
    // This method will be called to determine the state of the node based on the specified index
    public MultiTick MultiStateMethod;

    // Index used by the multi-state node
    public int index;

    // Empty constructor
    public BTLeaf() { }

    // Constructor for the node with a single state
    public BTLeaf(string name, Tick sm)
    {
        nodeName = name;
        StateMethod = sm;
    }

    // Constructor for the multi-state node
    public BTLeaf(string name, int i, MultiTick sm)
    {
        nodeName = name;
        MultiStateMethod = sm;
        index = i;
    }

    // Constructor for the node with a single state and a sort priority
    public BTLeaf(string name, Tick sm, int order)
    {
        nodeName = name;
        StateMethod = sm;
        sortPriority = order;
    }

    // Method to process the node and determine its state
    // If the node has a single state, its state is determined by calling its StateMethod
    // If the node is a multi-state node, its state is determined by calling its MultiStateMethod with the specified index
    public override NodeState Process()
    {
        BTNode.NodeState s;
        if (StateMethod != null)
            s = StateMethod();
        else if (MultiStateMethod != null)
            s = MultiStateMethod(index);
        else
            s = NodeState.FAILURE;
        UnityEngine.Debug.Log(nodeName + " " + s);
        return s;
    }
}
