using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{

    public BehaviourTree tree;
    public NavMeshAgent agent;

    //Define an enum for the agent's action state
    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;

    //Declare a variable to keep track of the state of the behaviour tree
    BTNode.NodeState treeState = BTNode.NodeState.RUNNING;


    WaitForSeconds waitForSeconds;
    Vector3 lastLocation;

    public virtual void Start()
    {
        
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree("Behaviour Tree");
        waitForSeconds = new WaitForSeconds(Random.Range(1, 5));
        StartCoroutine(Behave());
    }
    //Method to check if the agent can see a target
    //Parameters: target - the position of the target, tag - the tag of the target, distance - the distance the agent can see, maxAngle - the angle the agent can see
    //Returns: SUCCESS if the agent can see the target, FAILURE if the agent can't see the target
    //Note: This method uses a raycast to check if the agent can see the target, the raycast can only hit objects with the specified tag, within the specified distance and angle
    public BTNode.NodeState CanSee(Vector3 target, string tag, float distance, float maxAngle)
    {
        Vector3 directionToTarget = target - this.transform.position;
        float angle = Vector3.Angle(directionToTarget, this.transform.forward);
        Debug.DrawRay(this.transform.position, directionToTarget, Color.black);
        if (angle <= maxAngle || directionToTarget.magnitude <= distance)
        {
            Debug.DrawRay(this.transform.position, directionToTarget, Color.blue);
            if (Physics.Raycast(this.transform.position, directionToTarget, out RaycastHit hitInfo))
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

    //Causes the agent to run away from a threat
    public BTNode.NodeState Flee(Vector3 location, float distance)
    {
        if (state == ActionState.IDLE)
        {
            lastLocation = this.transform.position + (transform.position - location).normalized * distance;
        }
        return GoToLocation(lastLocation);
    }
    //Method to move the agent to a specified location
    public BTNode.NodeState GoToLocation(Vector3 location)
    {

        float distanceToLocation = Vector3.Distance(location, this.transform.position);
        // If the agent is idle, set the destination to the specified location and set the state to working
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(location);
            state = ActionState.WORKING;

        }
        //If the agent is more than 5 units away from the location, it is still moving towards it, so return running
        else if (Vector3.Distance(agent.pathEndPosition, location) >= 5)
        {
            state = ActionState.IDLE;
            //return BTNode.NodeState.FAILURE;
        }
        //If the agent is within the set distance, it has reached the location, so set the state to idle and return success
        else if (distanceToLocation <= 4)
        {
            state = ActionState.IDLE;
            return BTNode.NodeState.SUCCESS;
        }

        return BTNode.NodeState.RUNNING;
    }
    
    public BTNode.NodeState IsOpen()
    {
        // If the time of day is before the open time or after the close time, return failure
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

    public void Wait()
    {
        return;
    }

}
