using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class NPCBehaviour : BTAgent
{

 
    public GameObject goal;
    public GameObject home;
    public GameObject goal2;

    [Range(0, 100)]
    public int health = 75;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        BTSequence steal = new("Steal");
        BTLeaf hasHealth = new BTLeaf("Has Health", HasHealth);
        BTLeaf goToPos = new BTLeaf("Go To Position", GoToPosition);
        BTLeaf goToPos2 = new BTLeaf("Go To Position2", GoToPosition2);
        BTLeaf goToHome = new BTLeaf("Go To Home", GoToHome);
        BTSelector chooseGoal = new BTSelector("Choose Goal");

        BTInverter invertHealth = new("Invert Health");
        invertHealth.AddChild(hasHealth);

        chooseGoal.AddChild(goToPos);
        chooseGoal.AddChild(goToPos2);

        steal.AddChild(invertHealth);
        steal.AddChild(chooseGoal);
        steal.AddChild(goToHome);
        tree.AddChild(steal);

        tree.PrintTree();
    }

    public BTNode.NodeState HasHealth()
    {
        if(health < 50)
            return BTNode.NodeState.FAILURE;
        return BTNode.NodeState.SUCCESS;
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
        BTNode.NodeState s = GoToLocation(home.transform.position);
        if(s == BTNode.NodeState.SUCCESS)
        {
            health += 50;
        }
        return s;
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

    //BTNode.NodeState GoToLocation(Vector3 location)
    //{
    //    float distanceToLocation = Vector3.Distance(location, this.transform.position);
    //    if (state == ActionState.IDLE)
    //    {
    //        state = ActionState.WORKING;
    //        agent.SetDestination(location);
    //    }
    //    else if (Vector3.Distance(agent.pathEndPosition, location) >= 2)
    //    {
    //        state = ActionState.IDLE;
    //        return BTNode.NodeState.FAILURE;
    //    }
    //    else if (distanceToLocation <= 2)
    //    {
    //        state = ActionState.IDLE;
    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    return BTNode.NodeState.RUNNING;
    //}


}
