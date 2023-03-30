using System.Collections;
using System.Collections.Generic;
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

  
    public void Start()
    {        
        agent = this.GetComponent<NavMeshAgent>();        
        tree = new BehaviourTree("Behaviour Tree");       
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));      
        StartCoroutine(Behave());
    }

    // Method to move the agent to a specified location
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
