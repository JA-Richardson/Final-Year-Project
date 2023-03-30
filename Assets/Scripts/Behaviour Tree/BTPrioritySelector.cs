using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BTPrioritySelector : BTNode
{
    BTNode[] nodeArray;
    bool ordered = false;

    public BTPrioritySelector(string name) : base(name)
    {
        nodeName = name;
    }

    // This method orders the child nodes based on their sortPriority property
    void OrderBTNodes()
    {
        // Convert the list of child nodes to an array
        nodeArray = children.ToArray();

        // Sort the child nodes based on their sortPriority property using the QuickSort algorithm
        Sort(nodeArray, 0, children.Count - 1);

        // Convert the sorted array back to a list of child nodes
        children = new List<BTNode>(nodeArray);
    }

    // This method processes the child nodes in order based on their sortPriority property
    public override NodeState Process()
    {
        // If the child nodes haven't been ordered yet, order them first
        if (!ordered)
        {
            OrderBTNodes();
            ordered = true;
        }

        // Get the NodeState of the current child node being processed
        NodeState childState = children[currentChild].Process();

        // If the current child node is still running, return a NodeState of "RUNNING"
        if (childState == NodeState.RUNNING)
            return NodeState.RUNNING;

        // If the current child node succeeded in processing, set its sortPriority to 1, reset the currentChild variable to 0, and return a NodeState of "SUCCESS"
        if (childState == NodeState.SUCCESS)
        {
            children[currentChild].sortPriority = 1;
            currentChild = 0;
            ordered = false;
            return NodeState.SUCCESS;
        }
        else
        {
            // If the current child node failed in processing, set its sortPriority to 10 and move on to the next child node
            children[currentChild].sortPriority = 10;
        }
        currentChild++;

        // If all child nodes have been processed, reset the currentChild variable to 0, set ordered to false, and return a NodeState of "FAILURE"
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            ordered = false;
            return NodeState.FAILURE;
        }

        // If there are still child nodes to process, return a NodeState of "RUNNING"
        return NodeState.RUNNING;
    }

    //QuickSort
    //Adapted from: https://exceptionnotfound.net/quick-sort-csharp-the-sorting-algorithm-family-reunion/
    int Partition(BTNode[] array, int low,
                                int high)
    {
        BTNode pivot = array[high];

        int lowIndex = (low - 1);

        //2. Reorder the collection.
        for (int j = low; j < high; j++)
        {
            if (array[j].sortPriority <= pivot.sortPriority)
            {
                lowIndex++;

                BTNode temp = array[lowIndex];
                array[lowIndex] = array[j];
                array[j] = temp;
            }
        }

        BTNode temp1 = array[lowIndex + 1];
        array[lowIndex + 1] = array[high];
        array[high] = temp1;

        return lowIndex + 1;
    }

    void Sort(BTNode[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }

}
