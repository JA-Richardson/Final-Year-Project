using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdBehaviour : BTAgent
{
    public GameObject[] shelf;
    public GameObject door;
    public GameObject home;

    [Range(0, 1000)]
    public int hunger = 1000;

    public bool entryAllowed = false;
    public bool isWaiting = false;

    public override void Start()
    {
        base.Start();

        BTRandomSelector chooseShelf = new BTRandomSelector("Choose Goal");
        for (int i = 0; i < shelf.Length; i++)
        {
            BTLeaf goToPos = new BTLeaf("Go To Position" + shelf[i].name, i, GoToArt);
            chooseShelf.AddChild(goToPos);
        }

        BTLeaf goToDoor = new("Go To Door", GoToDoor);
        BTLeaf goToHome = new("Go To Home", GoToHome);
        BTLeaf isBored = new("Bored", IsBored);
        BTLeaf isOpen = new("Open", IsOpen);

        BTSequence move = new("Move");
        move.AddChild(isOpen);
        move.AddChild(isBored);
        move.AddChild(goToDoor);

        BTLeaf notChecked = new("Wait to be checked", NotChecked);
        BTLeaf isWaiting = new("Wait for worker", IsWaiting);

        BehaviourTree waitForCheck = new();
        waitForCheck.AddChild(notChecked);
        BTLoop getTicket = new("Getting ticket", waitForCheck);
        getTicket.AddChild(isWaiting);

        move.AddChild(getTicket);

        BehaviourTree whileBored = new();
        whileBored.AddChild(isBored);

        BTLoop looking = new("Loop", whileBored);
        looking.AddChild(chooseShelf);
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

    IEnumerator increaseHunger()
    {
        while(true)
        {
            hunger = Mathf.Clamp(hunger + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public BTNode.NodeState GoToArt(int i)
    {
        BTNode.NodeState s = GoToLocation(shelf[i].transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            hunger = Mathf.Clamp(hunger - 150, 0, 1000);
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
        if (hunger < 200)
        {
            return BTNode.NodeState.FAILURE;
        }
        else
            return BTNode.NodeState.SUCCESS;
    }

   public BTNode.NodeState NotChecked()
    {
        if (entryAllowed || IsOpen() == BTNode.NodeState.FAILURE)
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
