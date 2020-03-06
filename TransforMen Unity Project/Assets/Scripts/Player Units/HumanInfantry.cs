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
    //COST: 5 scrap 

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
    protected override void Update() {
        base.Update();
    }
}
