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
    public GameObject[] loot;
    public GameObject enemy;

    [Range(0, 100)]
    public int health = 75;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        BTSequence move = new("Move");
        BTLeaf hasHealth = new BTLeaf("Has Health", HasHealth);
        BTLeaf goToHome = new BTLeaf("Go To Home", GoToHome);
        BTRandomSelector chooseGoal = new BTRandomSelector("Choose Goal");

        for (int i = 0; i < loot.Length; i++)
        {
            BTLeaf goToPos = new BTLeaf("Go To Position" + loot[i].name, i, GoToPosition);
            chooseGoal.AddChild(goToPos);
        }

        BTInverter invertHealth = new("Invert Health");
        invertHealth.AddChild(hasHealth);
        // BTInverter cantSeeEnemy = new BTInverter("Can't see");
        //cantSeeEnemy.AddChild(cantSeeEnemy);

        move.AddChild(invertHealth);
     //   move.AddChild(cantSeeEnemy);
        move.AddChild(chooseGoal);
       // move.AddChild(cantSeeEnemy);
        move.AddChild(goToHome);

        tree.PrintTree();

        BTSequence runAway = new BTSequence("Run Away");
        BTLeaf canSee = new BTLeaf("Can see enemy?", CanSeeEnemy);
        BTLeaf flee = new BTLeaf("Run away", FleeFromEnemy);

        runAway.AddChild(canSee);
        runAway.AddChild(flee);

        BTSelector moveOrRun = new BTSelector("Move or run");
        moveOrRun.AddChild(move);
        moveOrRun.AddChild(runAway);

        tree.AddChild(moveOrRun);
        tree.PrintTree();
    }

    public BTNode.NodeState CanSeeEnemy()
    {
        return CanSee(enemy.transform.position, "Player", 10, 90);
    }
    
    public BTNode.NodeState FleeFromEnemy()
    {
        return Flee(enemy.transform.position, 10);
    }

    public BTNode.NodeState HasHealth()
    {
        if(health < 50)
            return BTNode.NodeState.FAILURE;
        return BTNode.NodeState.SUCCESS;
    }
    
    public BTNode.NodeState GoToPosition(int i)
    {
        BTNode.NodeState s = GoToLocation(loot[i].transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            return BTNode.NodeState.SUCCESS;
        }
        else
            return s;
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

}
