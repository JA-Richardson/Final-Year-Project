using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : BTNode
{

    // Struct to define the level and node in the behaviour tree
    struct BTNodeLevel
    {
        public BTNode node;
        public int level;
    }
    public BehaviourTree()
    {
        nodeName = "Behaviour Tree";
    }

    // Constructor to set node name to input argument
    public BehaviourTree(string name)
    {
        nodeName = name;
    }

    // Process function that overrides the BTNode's Process function
    // Returns the state of the current child node's processing
    public override NodeState Process()
    {
        if (children.Count == 0) return NodeState.SUCCESS;
        return children[currentChild].Process();
    }

    // Function to print out the behaviour tree structure
    public void PrintTree()
    {
  
        string treePrintout = "";
        // Define a stack to hold the current node and its level
        Stack<BTNodeLevel> stack = new Stack<BTNodeLevel>();
        // Set the current node to the root node of the behaviour tree
        BTNode currentNode = this;
        stack.Push(new BTNodeLevel { level = 0, node = currentNode });

        while (stack.Count != 0)
        {
            BTNodeLevel nextNode = stack.Pop();
            // Add the node's name to the tree printout string with the correct level of indentation
            treePrintout += new string('-', nextNode.level) + nextNode.node.nodeName + "\n";
            // Loop through the node's children from the last index to the first
            for (int i = nextNode.node.children.Count - 1; i >= 0; i--)
            {
                // Push the child node onto the stack with a level one higher than its parent
                stack.Push(new BTNodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i] });
            }
        }
        Debug.Log(treePrintout);
    }
}
