using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{

    public BehaviourTree tree;
    public NavMeshAgent agent;

    // Define an enum for the agent's action state
    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;

    // Declare a variable to keep track of the state of the behaviour tree
    BTNode.NodeState treeState = BTNode.NodeState.RUNNING;


    WaitForSeconds waitForSeconds;
    Vector3 lastLocation;


    public virtual void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree("Behaviour Tree");
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));
        StartCoroutine(Behave());
    }

    public BTNode.NodeState CanSee(Vector3 target, string tag, float distance, float maxAngle)
    {
        Vector3 directionToTarget = target - this.transform.position;
        float angle = Vector3.Angle(directionToTarget, this.transform.forward);
        Debug.DrawRay(this.transform.position, directionToTarget, Color.black);
        if (angle <= maxAngle || directionToTarget.magnitude <= distance)
        {
            RaycastHit hitInfo;
            Debug.DrawRay(this.transform.position, directionToTarget, Color.blue);
            if (Physics.Raycast(this.transform.position, directionToTarget, out hitInfo))
            {
                Debug.DrawRay(this.transform.position, directionToTarget, Color.red);
                if (hitInfo.collider.gameObject.CompareTag(tag))
                {
                    return BTNode.NodeState.SUCCESS;
                }
            }
        }
        return BTNode.NodeState.FAILURE;
    }

    public BTNode.NodeState Flee(Vector3 location, float distance)
    {
        if (state == ActionState.IDLE)
        {
            lastLocation = this.transform.position + (transform.position - location).normalized * distance;
        }
        return GoToLocation(lastLocation);
    }
    // Method to move the agent to a specified location
    public BTNode.NodeState GoToLocation(Vector3 location)
    {

        float distanceToLocation = Vector3.Distance(location, this.transform.position);

        if (state == ActionState.IDLE)
        {
            agent.SetDestination(location);
            state = ActionState.WORKING;
            
        }

        else if (Vector3.Distance(agent.pathEndPosition, location) >= 2)
        {
            state = ActionState.IDLE;
            return BTNode.NodeState.FAILURE;
        }

        else if (distanceToLocation <= 4)
        {
            state = ActionState.IDLE;
            return BTNode.NodeState.SUCCESS;
        }

        return BTNode.NodeState.RUNNING;
    }

    public BTNode.NodeState IsOpen()
    {
        if (Blackboard.Instance.timeOfDay < Blackboard.Instance.openTime || Blackboard.Instance.timeOfDay > Blackboard.Instance.closeTime)
        {
            return BTNode.NodeState.FAILURE;
        }
        else
            return BTNode.NodeState.SUCCESS;

    }

    // Coroutine to execute the behaviour tree
    IEnumerator Behave()
    {
        // Execute the behaviour tree indefinitely
        while (true)
        {
            // Update the state of the behaviour tree
            treeState = tree.Process();
            // Wait for the specified amount of time before continuing to execute the coroutine
            yield return waitForSeconds;
        }
    }

}
