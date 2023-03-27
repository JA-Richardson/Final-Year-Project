using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{

    public BehaviourTree tree;
    public NavMeshAgent agent; 
    
    public enum ActionState { IDLE, WORKING};
    public ActionState state = ActionState.IDLE;

    BTNode.NodeState treeState = BTNode.NodeState.RUNNING;

    WaitForSeconds waitForSeconds;

    // Start is called before the first frame update
    public void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree("Behaviour Tree");
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));
        StartCoroutine(Behave());
    }

    public BTNode.NodeState GoToLocation(Vector3 location)
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

    IEnumerator Behave()
    {
        while (true)
        {
            treeState = tree.Process();
            yield return waitForSeconds;
        }
    }

}
