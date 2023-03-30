using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
//Still needs commenting, do not forget
public class NPCBehaviour : BTAgent
{
    public GameObject home;
    public GameObject[] spaces;

    [Range(0, 100)]
    public int health = 75;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        BTSequence move = new("Move");
        BTLeaf hasHealth = new BTLeaf("Has Health", HasHealth);
        BTLeaf goToHome = new BTLeaf("Go To Home", GoToHome);
        
        //BTLeaf goToPos1 = new BTLeaf("Go To Position", GoToPosition1);
        //BTLeaf goToPos2 = new BTLeaf("Go To Position2", GoToPosition2);
        
        //BTPrioritySelector chooseGoal = new BTPrioritySelector("Choose Goal");

        //BTLeaf goToPos3 = new BTLeaf("Go To Position3", GoToPosition3);
        //BTLeaf goToPos4 = new BTLeaf("Go To Position4", GoToPosition4);
        //BTLeaf goToPos5 = new BTLeaf("Go To Position5", GoToPosition5);
        //BTLeaf goToPos6 = new BTLeaf("Go To Position6", GoToPosition6);
        //BTLeaf goToPos7 = new BTLeaf("Go To Position7", GoToPosition7);
        //BTLeaf goToPos8 = new BTLeaf("Go To Position8", GoToPosition8);
        //BTLeaf goToPos9 = new BTLeaf("Go To Position9", GoToPosition9);

        BTRandomSelector chooseGoal = new BTRandomSelector("Choose Goal");
        for (int i = 0; i < spaces.Length; i++)
        {
            BTLeaf goToPos = new BTLeaf("Go To Position" + spaces[i].name, i, GoToPosition);
            chooseGoal.AddChild(goToPos);
            //chooseGoal.AddChild(new BTLeaf("Go To Position" + i, () => GoToPosition(i)));
        }

        BTInverter invertHealth = new("Invert Health");
        invertHealth.AddChild(hasHealth);

        
        //chooseGoal.AddChild(goToPos1);
        //chooseGoal.AddChild(goToPos2);
        //chooseGoal.AddChild(goToPos3);
        //chooseGoal.AddChild(goToPos4);
        //chooseGoal.AddChild(goToPos5);
        //chooseGoal.AddChild(goToPos6);
        //chooseGoal.AddChild(goToPos7);
        //chooseGoal.AddChild(goToPos8);
        //chooseGoal.AddChild(goToPos9);

        move.AddChild(invertHealth);
        move.AddChild(chooseGoal);
        move.AddChild(goToHome);
        tree.AddChild(move);

        tree.PrintTree();
    }

    public BTNode.NodeState HasHealth()
    {
        if(health < 50)
            return BTNode.NodeState.FAILURE;
        return BTNode.NodeState.SUCCESS;
    }
    
    public BTNode.NodeState GoToPosition(int i)
    {
        BTNode.NodeState s = GoToLocation(spaces[i].transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            return BTNode.NodeState.SUCCESS;
        }
        else
            return s;
    }

    //public BTNode.NodeState GoToPosition1()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[0].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}

    //public BTNode.NodeState GoToPosition2()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[1].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}

    //public BTNode.NodeState GoToPosition3()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[2].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}
    //public BTNode.NodeState GoToPosition4()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[3].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}
    //public BTNode.NodeState GoToPosition5()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[4].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}
    //public BTNode.NodeState GoToPosition6()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[5].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}
    //public BTNode.NodeState GoToPosition7()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[6].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}
    //public BTNode.NodeState GoToPosition8()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[7].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}
    //public BTNode.NodeState GoToPosition9()
    //{
    //    BTNode.NodeState s = GoToLocation(spaces[8].transform.position);
    //    if (s == BTNode.NodeState.SUCCESS)
    //    {

    //        return BTNode.NodeState.SUCCESS;
    //    }
    //    else
    //        return s;
    //}

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
