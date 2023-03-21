using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : States
{
    float rotSpeed = 2.0f;
    public Attack(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer) : base(sNPC, sAgent, sPlayer)
    {
        name = STATE.ATTACK;
        agent.speed = 0;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
    }

    public override void Update()
    {

        base.Update();
        Vector3 dir = player.position - NPC.transform.position;
        float angle = Vector3.Angle(dir, NPC.transform.forward);
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation, lookRot, Time.deltaTime * rotSpeed);
        if (!CanAttackPlayer())
        {
            nextState = new Pursue(NPC, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
