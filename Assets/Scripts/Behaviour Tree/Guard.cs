using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : BTAgent
{
    public GameObject[] patrolPath;
    public GameObject thief;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        BTSequence patrolArea = new("Patrol");
        for (int i = 0; i < patrolPath.Length; i++)
        {
            BTLeaf goToPos = new BTLeaf("Go To Position" + patrolPath[i].name, i, GoToPoint);
            patrolArea.AddChild(goToPos);
        }

        BTSequence chaseThief = new("Chase thief");
        BTLeaf canSee = new("Can see thief", CanSeeThief);
        BTLeaf chase = new("Chase thief", ChaseThief);

        chaseThief.AddChild(canSee);
        chaseThief.AddChild(chase);

        BTInverter cantSeeThief = new("Can't see thief");
        cantSeeThief.AddChild(canSee);

        BehaviourTree patrolCondition = new();
        BTSequence condition = new("Patrol condition");
        condition.AddChild(cantSeeThief);
        patrolCondition.AddChild(condition);

        BTDependencySequence patrol = new("Patrol", patrolCondition, agent);
        patrol.AddChild(patrolArea);

        BTSelector areaGuard = new("Guard");
        areaGuard.AddChild(patrol);
        areaGuard.AddChild(chaseThief);

        tree.AddChild(areaGuard);
    }

    public BTNode.NodeState GoToPoint(int index)
    {
        BTNode.NodeState s = GoToLocation(patrolPath[index].transform.position);
        return s;
    }

    public BTNode.NodeState CanSeeThief()
    {
        return CanSee(thief.transform.position, "Thief", 5, 60);
    }

    Vector3 lastThiefPos;

    public BTNode.NodeState ChaseThief()
    {
        float chaseDistance = 10;
        if(state == ActionState.IDLE)
        {
            lastThiefPos = this.transform.position - (transform.position - thief.transform.position).normalized * chaseDistance;
        }
        return GoToLocation(lastThiefPos);
    }
}
