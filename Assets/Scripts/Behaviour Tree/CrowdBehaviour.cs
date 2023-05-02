using System.Collections;
using UnityEngine;

public class CrowdBehaviour : BTAgent
{
    public GameObject[] shelf;
    public GameObject door;
    public GameObject home;
    public GameObject checkout;
    public int shelfIndex = 0;

    [Range(0, 1000)]
    public int hunger = 0;

    public bool entryAllowed = false;
    public bool isWaiting = false;
    
    public override void Start()
    {
        base.Start();

        BTRandomSelector chooseShelf = new("Choose Goal");
        for (int i = 0; i < shelf.Length; i++)
        {
            BTLeaf goToPos = new("Go To Position" + shelf[i].name, i, GoToShelf);
            chooseShelf.AddChild(goToPos);
            
        }

        BTLeaf goToDoor = new("Go To Door", GoToDoor);
        BTLeaf goToHome = new("Go To Home", GoToHome);
        BTLeaf isHungry = new("Hungry", IsHungry);
        BTLeaf isOpen = new("Open", IsOpen);
        BTLeaf checkout = new("Checkout", Checkout);

        BTSequence move = new("Move");
        move.AddChild(isOpen);
        move.AddChild(isHungry);
        move.AddChild(goToDoor);

        BTLeaf notChecked = new("Wait to be checked", NotChecked);
        BTLeaf isWaiting = new("Wait for worker", IsWaiting);

        BehaviourTree waitForCheck = new();
        waitForCheck.AddChild(notChecked);
        BTLoop getTicket = new("Getting ticket", waitForCheck);
        getTicket.AddChild(isWaiting);

        move.AddChild(getTicket);

        BehaviourTree whileHungry = new();
        whileHungry.AddChild(isHungry);

        BTLoop looking = new("Loop", whileHungry);
        looking.AddChild(chooseShelf);
        
        move.AddChild(looking);
        move.AddChild(checkout);
        move.AddChild(goToHome);

        BehaviourTree areaOpen = new();
        areaOpen.AddChild(isOpen);

        BTDependencySequence viewProduce = new("View Produce", areaOpen, agent);
        viewProduce.AddChild(move);

        BTSelector fallback = new("Fallback");
        fallback.AddChild(viewProduce);
        fallback.AddChild(goToHome);

        tree.AddChild(fallback);
        StartCoroutine(nameof(IncreaseHunger));
    }
    // Coroutine to increase hunger
    IEnumerator IncreaseHunger()
    {
        while (true)
        {
            hunger = Mathf.Clamp(hunger + 10, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }
    //If the agent has visited 5 or more shelves they will go to the checkout
    public BTNode.NodeState Checkout()
    {
        if (shelfIndex >= 5)
        {
            BTNode.NodeState s = GoToLocation(checkout.transform.position);
            if (s == BTNode.NodeState.SUCCESS)
            {
                shelfIndex = 0;
                hunger = 0;
                return BTNode.NodeState.SUCCESS;
            }
            else
                return s;
        }
        else
        {
            return BTNode.NodeState.FAILURE;
        }
    }
    
    public BTNode.NodeState GoToShelf(int i)
    {
        BTNode.NodeState s = GoToLocation(shelf[i].transform.position);
        if (s == BTNode.NodeState.SUCCESS)
        {
            hunger = Mathf.Clamp(hunger - 200, 0, 1000);
            shelfIndex++;
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

    public BTNode.NodeState IsHungry()
    {
        if (hunger < 200)
        {
            return BTNode.NodeState.FAILURE;
        }
        else
            return BTNode.NodeState.SUCCESS;
    }
    //Agent waits to be allowed entry to the store
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
