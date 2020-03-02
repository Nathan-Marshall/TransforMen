using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Human Infantry: basic player controlled dynamic unit which 
//                 cam attack, move and be attacked 
//-------------------------------------------------------------

public class HumanInfantry : AttackUnit
{
    private string desc = "A basic person fighting for their planet";

    //Collaborators: Dynamic Unit, Attack Target, Attack Unit 

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);
        SetTransportable(true);
        SetMoveSpeed(5);
        SetCanScavenge(true);
        SetControllable(true);
        SetCanAttack(true);
        SetWeapon(new MachineGun());
        SetAttackTarget(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
