using UnityEngine;

public class BTWorker : BTAgent
{
    public GameObject booth;
    public GameObject crowd;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        BTLeaf allocateCustomer = new("Allocate customer", AllocateCustomer);
        BTLeaf goToCustomer = new("Go To Customer", GoToCrowdMember);
        BTLeaf goToBooth = new("Go To Booth", GoToBooth);
        BTLeaf customerWaiting = new("Customer Waiting", CustomerWaiting);


        BTSequence allocate = new("Allocate");
        allocate.AddChild(allocateCustomer);

        BehaviourTree waiting = new();
        waiting.AddChild(customerWaiting);

        BTDependencySequence goingToCustomer = new("Going To Customer", waiting, agent);
        goingToCustomer.AddChild(goToCustomer);


        allocate.AddChild(goingToCustomer);

        BTSelector work = new("Work");
        work.AddChild(allocate);
        work.AddChild(goToBooth);

        tree.AddChild(work);

    }


    public BTNode.NodeState GoToCrowdMember() // Go to the crowd member and allow them to enter the store
    {
        
        if (crowd == null) return BTNode.NodeState.FAILURE;
        BTNode.NodeState s = GoToLocation(crowd.transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            crowd.GetComponent<CrowdBehaviour>().entryAllowed = true;
            crowd = null;

        }
        return s;
    }

    public BTNode.NodeState GoToBooth()
    {
        BTNode.NodeState s = GoToLocation(booth.transform.position);
        crowd = null;
        return s;
    }

    public BTNode.NodeState AllocateCustomer() //Allocates a customer from the list of waiting agents
    {
        if (Blackboard.Instance.crowd.Count == 0)
            return BTNode.NodeState.FAILURE;
        crowd = Blackboard.Instance.crowd.Pop();
        if (crowd == null)
            return BTNode.NodeState.FAILURE;
        return BTNode.NodeState.SUCCESS;
    }

    public BTNode.NodeState CustomerWaiting() // Checks to see if anyone is waiting to be allowed entry to the store
    {
        if (crowd == null)
            return BTNode.NodeState.FAILURE;
        if (crowd.GetComponent<CrowdBehaviour>().isWaiting)
            return BTNode.NodeState.SUCCESS;
        return BTNode.NodeState.FAILURE;
    }
}

