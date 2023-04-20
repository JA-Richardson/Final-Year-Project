using System.Collections.Generic;

public class BTNode
{
    // Enum representing the possible states of the node
    public enum NodeState { SUCCESS, FAILURE, RUNNING };
    public NodeState nodeState;
    public List<BTNode> children = new();
    public int currentChild = 0;
    public string nodeName;
    public int sortPriority;

    // Default constructor
    public BTNode() { }

    // Constructor taking only the name of the node
    public BTNode(string name)
    {
        nodeName = name;
    }

    // Constructor taking the name and priority of the node
    public BTNode(string name, int order)
    {
        nodeName = name;
        sortPriority = order;
    }

    // Method used to process the node and its children
    public virtual NodeState Process()
    {
        return children[currentChild].Process();
    }

    // Method used to add a child node to the list
    public void AddChild(BTNode child)
    {
        children.Add(child);
    }

    public void Reset()
    {
        foreach (BTNode child in children)
        {
            child.Reset();
        }
        currentChild = 0;
    }
}

