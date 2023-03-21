using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : States
{
    public Pursue(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer) : base(sNPC, sAgent, sPlayer)
    {
        name = STATE.CHASE;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public override void Update()
    {
        base.Update();
        agent.SetDestination(player.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(NPC, agent, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(NPC, agent, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}