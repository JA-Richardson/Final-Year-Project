using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{

    BehaviourTree tree;
    // Start is called before the first frame update
    void Start()
    {
        tree = new BehaviourTree("Behaviour Tree");
        BTNode steal = new BTNode("Steal");
        BTNode goToPos = new BTNode("Go To Position");
        BTNode goToHome = new BTNode("Go To Home");

        steal.AddChild(goToPos);
        steal.AddChild(goToHome);
        tree.AddChild(steal);

        BTNode walk = new BTNode("walk");
        BTNode run = new BTNode("run");
        BTNode jog = new BTNode("jog");

        walk.AddChild(run);
        walk.AddChild(jog);
        tree.AddChild(walk);

        tree.PrintTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
