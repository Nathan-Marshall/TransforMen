using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Spike Dude: Upgraded version of human infantry. Can attack,
//     move, be attacked. Also has an on hit action to damage
//     its attacker. Upgradeable.
//-------------------------------------------------------------

public class SpikeDude : HumanInfantry
{
    private string desc = "A genetically upgraded human who attacks with spikes";
    //Collaborators: Dynamic Unit, Attack Target, Attacker, Human Infantry(?)
    //COST: 1 spike

    //On-hit action (dmg attacker)
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
        SetWeapon(new SpikeShooter());
        SetAttackTarget(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
