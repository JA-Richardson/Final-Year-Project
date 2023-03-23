using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : States
{
    public Idle(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer) : base(sNPC, sAgent, sPlayer)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;

    }

    public override void Update()
    {
        base.Update();
        if (CanSeePlayer())
        {
            nextState = new Pursue(NPC, agent, player);
            stage = EVENT.EXIT;
        }
        else /*if(Random.Range(0, 100) < 10)*/
        {
            nextState = new Patrol(NPC, agent, player);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
