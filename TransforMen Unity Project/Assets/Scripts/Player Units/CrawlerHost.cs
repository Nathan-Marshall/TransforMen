using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Crawler Host: Upgraded version of human infantry. Can attack
//     move, be attacked. Upon death, crawler host spawns 
//     crawler units. Is upgradeable.
//-------------------------------------------------------------

public class CrawlerHost : HumanInfantry
{
    private string desc = "A genetically upgraded person fighting for their planet";
    //Collaborators: Dynamic Unit, Attack Target, Attack Unit, Crawler Spawn 
    //COST: 1

    //On-death action
    bool upgradeable; //whether or not current can be upgraded

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);
        SetTransportable(true);
        SetMoveSpeed(5);
        SetCanScavenge(false);
        SetControllable(true);
        SetCanAttack(true);
        SetWeapon(new Melee());
        SetAttackTarget(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
