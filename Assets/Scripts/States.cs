using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class States
{
    public enum STATE { IDLE, PATROL, CHASE, ATTACK, DEAD };

    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    public GameObject NPC;
    public Transform player;
    protected NavMeshAgent agent;
    public States nextState;

    public float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 2.0f;

    public States(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer)
    {
        NPC = sNPC;
        agent = sAgent;
        this.player = sPlayer;
        stage = EVENT.ENTER;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public States Process()
    {
        if (stage == EVENT.ENTER)
        {
            Enter();
        }
        if (stage == EVENT.UPDATE)
        {
            Update();
        }
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }


}

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
        if (Random.Range(0, 100) < 10)
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
        base.Enter();
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= EnvironmentManager.Singleton.PatrolPoints.Count -1)
            {
                currentIndex = 0;
            }
            else
                currentIndex++;
            agent.SetDestination(EnvironmentManager.Singleton.PatrolPoints[currentIndex].transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
