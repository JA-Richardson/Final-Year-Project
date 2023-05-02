using UnityEngine;
//Still needs commenting, do not forget
public class NPCBehaviour : BTAgent
{
    public GameObject home;
    public GameObject[] loot;
    public GameObject enemy;
    public GameObject painting;
    public GameObject pickup;

    [Range(0, 100)]
    public int health = 75;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // Behaviour tree setup
        BTLeaf hasHealth = new("Has Health", HasHealth);
        BTLeaf goToHome = new("Go To Home", GoToHome);
        BTLeaf goToHome2 = new("Go To Home2", GoToHome);
        BTLeaf goToPainting = new("Go To Painting", GoToPainting);
        BTLeaf canSee = new("Can see enemy?", CanSeeEnemy);
        BTLeaf flee = new("Run away", FleeFromEnemy);
        BTLeaf isOpen = new("is open", IsOpen);
        BTInverter isClosed = new("is closed");
        isClosed.AddChild(isOpen);

        //Old behaviour were the agent would look at other items to take before finally choosing one
        //Left in the tree as I planned on expanding it and giving the agent more objects to choose from
        BTRandomSelector chooseGoal = new("Choose Goal");
        for (int i = 0; i < loot.Length; i++)
        {
            BTLeaf goToPos = new("Go To Position" + loot[i].name, i, GoToPosition);
            chooseGoal.AddChild(goToPos);
        }

        BTInverter invertHealth = new("Invert Health");
        invertHealth.AddChild(hasHealth);
        BTInverter cantSeeEnemy = new("Can't see");
        cantSeeEnemy.AddChild(canSee);
        //The agent can only move if the store is closed, it can't see the enemy and it is below a set amount of health
        BehaviourTree moveConditions = new();
        BTSequence conditions = new("Move coniditions");
        conditions.AddChild(isClosed);
        conditions.AddChild(cantSeeEnemy);
        conditions.AddChild(invertHealth);
        moveConditions.AddChild(conditions);
        // The agent can only move if the conditions are met
        BTDependencySequence move = new("Move", moveConditions, agent);
        move.AddChild(chooseGoal);
        move.AddChild(goToPainting);
        move.AddChild(goToHome);

        // The agent runs away if it see's the store guard
        BTSequence runAway = new("Run Away");
        runAway.AddChild(canSee);
        runAway.AddChild(flee);
        // The agent chooses between attempting to steal the painting or leaving
        BTSelector grabOrLeave = new("Grab or Leave");
        grabOrLeave.AddChild(move);
        grabOrLeave.AddChild(goToHome);
        // The agent chooses between starting his theft behaviour or running away
        BTSelector moveOrRun = new("Move or run");
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

    public BTNode.NodeState GoToPainting()
    {
        if (!painting.activeSelf)
            return BTNode.NodeState.FAILURE;
        BTNode.NodeState s = GoToLocation(painting.transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            painting.transform.parent = this.gameObject.transform;
            pickup = painting;

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
