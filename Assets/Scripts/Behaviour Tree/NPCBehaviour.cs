using UnityEngine;
//Still needs commenting, do not forget
public class NPCBehaviour : BTAgent
{
    public GameObject home;
    public GameObject[] loot;
    public GameObject enemy;
    public GameObject crown;
    public GameObject pickup;

    [Range(0, 100)]
    public int health = 75;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        BTLeaf hasHealth = new BTLeaf("Has Health", HasHealth);
        BTLeaf goToHome = new BTLeaf("Go To Home", GoToHome);
        BTLeaf goToHome2 = new BTLeaf("Go To Home2", GoToHome);
        BTLeaf goToCrown = new BTLeaf("Go To Crown", GoToCrown);
        BTLeaf canSee = new BTLeaf("Can see enemy?", CanSeeEnemy);
        BTLeaf flee = new BTLeaf("Run away", FleeFromEnemy);
        BTLeaf isOpen = new("is open", IsOpen);
        BTInverter isClosed = new("is closed");
        isClosed.AddChild(isOpen);


        BTRandomSelector chooseGoal = new BTRandomSelector("Choose Goal");
        for (int i = 0; i < loot.Length; i++)
        {
            BTLeaf goToPos = new BTLeaf("Go To Position" + loot[i].name, i, GoToPosition);
            chooseGoal.AddChild(goToPos);
        }

        BTInverter invertHealth = new("Invert Health");
        invertHealth.AddChild(hasHealth);
        BTInverter cantSeeEnemy = new BTInverter("Can't see");
        cantSeeEnemy.AddChild(canSee);

        BehaviourTree moveConditions = new BehaviourTree();
        BTSequence conditions = new("Move coniditions");
        conditions.AddChild(isClosed);
        conditions.AddChild(cantSeeEnemy);
        conditions.AddChild(invertHealth);
        moveConditions.AddChild(conditions);

        BTDependencySequence move = new("Move", moveConditions, agent);
        move.AddChild(chooseGoal);
        move.AddChild(goToCrown);
        move.AddChild(goToHome);

        BTSequence runAway = new BTSequence("Run Away");
        runAway.AddChild(canSee);
        runAway.AddChild(flee);
        
        BTSelector grabOrLeave = new BTSelector("Grab or Leave");
        grabOrLeave.AddChild(move);
        grabOrLeave.AddChild(goToHome);

        BTSelector moveOrRun = new BTSelector("Move or run");
        moveOrRun.AddChild(grabOrLeave);
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
        if (health < 50)
            return BTNode.NodeState.FAILURE;
        return BTNode.NodeState.SUCCESS;
    }

    public BTNode.NodeState GoToCrown()
    {
        if (!crown.activeSelf)
            return BTNode.NodeState.FAILURE;
        BTNode.NodeState s = GoToLocation(crown.transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            crown.transform.parent = this.gameObject.transform;
            pickup = crown;

        }
        return s;
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
        if (s == BTNode.NodeState.SUCCESS)
        {
            if (pickup != null)
            {
                health += 50;
                pickup.SetActive(false);
                pickup = null;
            }
            
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
