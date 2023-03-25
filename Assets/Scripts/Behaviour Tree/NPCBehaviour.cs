using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehaviour : MonoBehaviour
{

    BehaviourTree tree;
    public GameObject goal;
    public GameObject home;
    NavMeshAgent agent; 
    
    public enum ActionState { IDLE, WORKING};
    ActionState state = ActionState.IDLE;

    BTNode.NodeState treeState = BTNode.NodeState.RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree("Behaviour Tree");
        BTSequence steal = new BTSequence("Steal");
        BTLeaf goToPos = new BTLeaf("Go To Position", GoToPosition);
        BTLeaf goToHome = new BTLeaf("Go To Home", GoToHome);

        steal.AddChild(goToPos);
        steal.AddChild(goToHome);
        tree.AddChild(steal);

        tree.PrintTree();
        
    }

    public BTNode.NodeState GoToPosition()
    {
        return GoToLocation(goal.transform.position);
    }

    public BTNode.NodeState GoToHome()
    {
        return GoToLocation(home.transform.position);
    }

    BTNode.NodeState GoToLocation(Vector3 location)
    {
        float distanceToLocation = Vector3.Distance(location, this.transform.position);
        if (state == ActionState.IDLE)
        {
            state = ActionState.WORKING;
            agent.SetDestination(location);
        }
        else if (Vector3.Distance(agent.pathEndPosition, location) >= 2)
        {
            state = ActionState.IDLE;
            return BTNode.NodeState.FAILURE;
        }
        else if (distanceToLocation <= 2)
        {
            state = ActionState.IDLE;
            return BTNode.NodeState.SUCCESS;
        }

        return BTNode.NodeState.RUNNING;
    }

    // Update is called once per frame
    void Update()
    {
        if (treeState == BTNode.NodeState.RUNNING)
            treeState = tree.Process();
    }
}
