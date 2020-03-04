using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Alien Crawler Spawn: dynamic unit that can attack human targets. 
//    is a simple melee unit that deals moderate damage 
//-------------------------------------------------------------

public class AlienCrawlerSpawn : AttackUnit
{
    private string desc = "Baby aliens who follow their mother";
    //Collaborators: Dynamic Unit, Attack Target, Attack Unit 

    //AI-Controlled

    // Start is called before the first frame update
    void Start()
    {
        SetToAlienTeam();
        SetSelectable(false);
        SetDescription(desc);
        SetTransportable(true);
        SetMoveSpeed(5);
        SetCanScavenge(false);
        SetControllable(false);
        SetCanAttack(true);
        SetWeapon(new Melee());
        SetAttackTarget(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
