using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehaviour : MonoBehaviour
{

    BehaviourTree tree;
    public GameObject goal;
    public GameObject home;
    public GameObject goal2;
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
        BTLeaf goToPos2 = new BTLeaf("Go To Position2", GoToPosition2);
        BTLeaf goToHome = new BTLeaf("Go To Home", GoToHome);
        BTSelector chooseGoal = new BTSelector("Choose Goal");

        chooseGoal.AddChild(goToPos);
        chooseGoal.AddChild(goToPos2);

        steal.AddChild(chooseGoal);
        steal.AddChild(goToHome);
        tree.AddChild(steal);

        tree.PrintTree();
    }

    public BTNode.NodeState GoToPosition()
    {
        return GoToFreeSpace(goal);
    }

    public BTNode.NodeState GoToPosition2()
    {
        return GoToFreeSpace(goal2);
    }

    public BTNode.NodeState GoToHome()
    {
        return GoToLocation(home.transform.position);
    }

    public BTNode.NodeState GoToFreeSpace(GameObject space)
    {
        BTNode.NodeState s = GoToLocation(space.transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            if (!space.GetComponent<FreeSpace>().freeSpace)
            {
                space.SetActive(false);
                return BTNode.NodeState.SUCCESS;
            }
            return BTNode.NodeState.FAILURE;
        }
        else
            return s;
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
