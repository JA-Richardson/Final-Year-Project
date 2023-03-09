using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States
{
    public enum STATE { IDLE, PATROL, CHASE, ATTACK, DEAD };

    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    public GameObject NPC;
    public Transform player;
    protected UnityEngine.AI.NavMeshAgent agent;
    public States nextState;

    float visDist = 10.0f;
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
    }


}
