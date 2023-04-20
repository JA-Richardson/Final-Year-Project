using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdBehaviour : BTAgent
{
    public GameObject[] art;
    public GameObject door;
    public GameObject home;

    [Range(0, 1000)]
    public int bored = 1000;

    public bool ticket = false;
    public bool isWaiting = false;

    public override void Start()
    {
        base.Start();

        BTRandomSelector chooseArt = new BTRandomSelector("Choose Goal");
        for (int i = 0; i < art.Length; i++)
        {
            BTLeaf goToPos = new BTLeaf("Go To Position" + art[i].name, i, GoToArt);
            chooseArt.AddChild(goToPos);
        }

        BTLeaf goToDoor = new("Go To Door", GoToDoor);
        BTLeaf goToHome = new("Go To Home", GoToHome);
        BTLeaf isBored = new("Bored", IsBored);
        BTLeaf isOpen = new("Open", IsOpen);

        BTSequence move = new("Move");
        move.AddChild(isOpen);
        move.AddChild(isBored);
        move.AddChild(goToDoor);

        BTLeaf noTicket = new("Get Ticket", NoTicket);
        BTLeaf isWaiting = new("Wait for worker", IsWaiting);

        BehaviourTree buyTicket = new();
        buyTicket.AddChild(noTicket);
        BTLoop getTicket = new("Getting ticket", buyTicket);
        getTicket.AddChild(isWaiting);

        move.AddChild(getTicket);

        BehaviourTree whileBored = new();
        whileBored.AddChild(isBored);

        BTLoop looking = new("Loop", whileBored);
        looking.AddChild(chooseArt);
        move.AddChild(looking);
        move.AddChild(goToHome);

        BehaviourTree areaOpen = new();
        areaOpen.AddChild(isOpen);

        BTDependencySequence viewArt = new("View Art", areaOpen, agent);
        viewArt.AddChild(move);

        BTSelector viewWithFallback = new("View With Fallback");
        viewWithFallback.AddChild(viewArt);
        viewWithFallback.AddChild(goToHome);

        tree.AddChild(viewWithFallback);
        StartCoroutine("increasedBored");
    }

    IEnumerator increasedBored()
    {
        while(true)
        {
            bored = Mathf.Clamp(bored + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public BTNode.NodeState GoToArt(int i)
    {
        BTNode.NodeState s = GoToLocation(art[i].transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            bored = Mathf.Clamp(bored - 150, 0, 1000);
            return BTNode.NodeState.SUCCESS;
            
        }
        else
            return s;
    }

    public BTNode.NodeState GoToDoor()
    {
        BTNode.NodeState s = GoToLocation(door.transform.position);
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
            isWaiting = false;
            return BTNode.NodeState.SUCCESS;
        }
        else
            return s;
    }

    public BTNode.NodeState IsBored()
    {
        if (bored < 200)
        {
            return BTNode.NodeState.FAILURE;
        }
        else
            return BTNode.NodeState.SUCCESS;
    }

   public BTNode.NodeState NoTicket()
    {
        if (ticket || IsOpen() == BTNode.NodeState.FAILURE)
        {
            return BTNode.NodeState.FAILURE;
        }
        else
            return BTNode.NodeState.SUCCESS;
    }

    public BTNode.NodeState IsWaiting()
    {
        if (Blackboard.Instance.RegisterCustomer(this.gameObject))
        {
            isWaiting = true;
            return BTNode.NodeState.SUCCESS;
        }
        else
            return BTNode.NodeState.FAILURE;
    }

}
