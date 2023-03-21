using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : States
{
    int currentIndex = -1;
    public Patrol(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer) : base(sNPC, sAgent, sPlayer)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < EnvironmentManager.Singleton.PatrolPoints.Count; i++)
        {
            GameObject thisPoint = EnvironmentManager.Singleton.PatrolPoints[i];
            float dist = Vector3.Distance(NPC.transform.position, thisPoint.transform.position);
            if (dist < lastDist)
            {
                currentIndex = i;
                lastDist = dist;
            }
        }
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= EnvironmentManager.Singleton.PatrolPoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
                currentIndex++;
            agent.SetDestination(EnvironmentManager.Singleton.PatrolPoints[currentIndex].transform.position);
        }
        if (CanSeePlayer())
        {
            nextState = new Pursue(NPC, agent, player);
            stage = EVENT.EXIT;
        }
        else if(IsPlayerBehind())
        {
            nextState = new Flee(NPC, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
